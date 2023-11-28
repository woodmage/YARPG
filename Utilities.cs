using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YARPG
{
    [Flags]
    public enum TargetTypes
    {
        None = 0,
        Single = 1,
        Multiple = 2,
        Point = 4,
        //add more if needed
    }

    public static class TargetingSystem
    {
        private static TargetTypes targettype = TargetTypes.None;
        private static Thing singletarget = new();
        private static List<Thing> multipletarget = new();
        private static Point targetpoint;

        public static void Clear()
        {
            targettype = TargetTypes.None; //clear targettype
            singletarget = new(); //clear singletarget
            multipletarget = new(); //clear multipletarget
            targetpoint = Point.Empty; //clear positionaltarget
        }
        public static void Status(string desc)
        {
            Info.Out($"{desc}:  "); //display desc
            Status(); //get status
        }
        public static void Status()
        {
            string buffer = ""; //set up an empty buffer
            if (IsNone()) //if there are no targets
            {
                buffer = "No"; //set the buffer to no
            }
            if ((targettype & TargetTypes.Point) != 0) //if point target defined
            {
                buffer += $"Point {targetpoint}"; //add that to buffer
            }
            if ((targettype & TargetTypes.Single) != 0) //if single target defined
            {
                buffer += $", Single ({singletarget.Name})"; //add that to buffer
            }
            if ((targettype & TargetTypes.Multiple) != 0) //if multiple targets defined
            {
                buffer += $", Multiple ({multipletarget.Count}: "; //add that to buffer
                foreach (var t in multipletarget) //for each target in multiple target
                {
                    buffer += $"{t.Name} "; //add target's name to buffer
                }
                buffer += ")"; //add close parentheses to buffer
            }
            buffer += " targets defined."; //add info to buffer
            Info.OutL($"{buffer}"); //display buffer
        }
        public static void Add(Point point)
        {
            Status("Begin Targeting"); //display beginning status
            List<Thing> targets = GameData.CurrentLevel.Creatures.Where(c => c.X == point.X && c.Y == point.Y).ToList(); //get list of creatures at the point
            if (targets.Count > 0) //if there are any targets
            {
                multipletarget.AddRange(targets); //add targets to multipletarget
                singletarget = multipletarget[0]; //set singletarget to first target
                if (multipletarget.Count > 1) //if there is more than one target
                {
                    targettype |= TargetTypes.Single; //set targettype to single
                    targettype |= TargetTypes.Multiple; //set targettype to multiple
                }
                else //otherwise
                {
                    targettype |= TargetTypes.Single; //just set targettype to single
                }
            }
            if (!IsPoint()) //if we don't have a point stored
            {
                targettype |= TargetTypes.Point; //set targettype to point
                targetpoint = point; //set positionaltarget to point
            }
            Status("End Targeting"); //display ending status
        }
        public static void Remove(Thing thing)
        {
            if (IsMultiple()) //if multiple targets
            {
                if (multipletarget.Contains(thing)) //if multiple target contains thing
                {
                    multipletarget.Remove(thing); //remove thing from multiple target
                }
            }
            if (singletarget == thing) //if single target equals thing
            {
                if (IsMultiple() && multipletarget.Count > 0) //if multiple target and there are multiple targets
                {
                    singletarget = multipletarget[0]; //set new single target
                }
            }
            if (IsMultiple()) //if multiple targets
            {
                if (multipletarget.Count == 0) //if there are no multiple targets
                {
                    targettype = TargetTypes.Single | TargetTypes.Point; //downgrade target type to single target
                }
            }
            if (!IsMultiple()) //if not multiple targets
            {
                targettype = TargetTypes.Point; //downgrade target type to point
            }
        }
        public static TargetTypes GetTargetType() => targettype;
        public static bool IsNone() => targettype == TargetTypes.None;
        public static bool IsSingle() => (targettype & TargetTypes.Single) != 0;
        public static bool IsMultiple() => (targettype & TargetTypes.Multiple) != 0;
        public static bool IsPoint() => (targettype & TargetTypes.Point) != 0;
        public static Point GetPoint() => targetpoint;
        public static Thing GetSingle() => singletarget;
        public static List<Thing> GetMultiple() => multipletarget;
        public static bool IsTargetted(Thing maybetarget)
        {
            if (IsNone()) //if there are no targets
            {
                return false; //report maybetarget is not a target
            }
            if (IsMultiple()) //if we have multiple creatures as targets
            {
                if (multipletarget.Contains(maybetarget)) //if maybetarget is one of them
                {
                    return true; //report maybetarget is a target
                }
            }
            if (IsSingle()) //if we have a single creature target
            {
                if (singletarget == maybetarget) //if singletarget is the same as maybetarget
                {
                    return true; //report maybetarget is a target
                }
            }
            return false; //report maybetarget is not a target
        }
        public static List<Thing> GetWithinArea(int radius)
        {
            multipletarget.Clear(); //clear the multiple target list
            if (!IsPoint()) //if we don't have a point,
            {
                return multipletarget; //return the empty list
            }
            for (int i = targetpoint.X - radius; i <= targetpoint.X + radius; i++) //for all points in x
            {
                for (int j = targetpoint.Y - radius; j <= targetpoint.Y + radius; j++) //for all points in y
                {
                    if (Math.Pow(i - targetpoint.X, 2) + Math.Pow(j - targetpoint.Y, 2) <= Math.Pow(radius, 2)) //if points are inside circle (use circle equation)
                    {
                        Add(new(i, j)); //add point (this is one reason adding a point doesn't overwrite a point!)
                    }
                }
            }
            targettype |= TargetTypes.Multiple; //set target type to multiple
            return multipletarget; // return the list
        }
    }

    public static class SoundSystem
    {
        public static void PlayEmbeddedSound(string resourceName)
        {
            if (Settings.DoSounds)
            {
                try
                {
                    Assembly currentAssembly = Assembly.GetExecutingAssembly();
                    using Stream? resourceStream = currentAssembly.GetManifestResourceStream(resourceName);
                    {
                        if (resourceStream == null)
                        {
                            MessageBox.Show("Resource not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        using SoundPlayer player = new(resourceStream);
                        {
                            player.Play();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing embedded sound: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void PlaySound(string soundFilePath)
        {
            if (Settings.DoSounds)
            {
                try
                {
                    using SoundPlayer player = new(soundFilePath);
                    {
                        player.Play();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
