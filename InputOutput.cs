namespace YARPG
{
    public static class InputOutput
    {
        /// <summary>
        /// Save - this method saves an Attack using a Streamwriter
        /// </summary>
        /// <param name="attack"></param>
        /// <param name="sw"></param>
        public static void Save(Attack attack, StreamWriter sw)
        {
            Writer w = new(sw); //get new Writer class
            w.WriteWords("Name", attack.Name); //write Name
            w.WriteInt("ToHit", attack.ToHit); //write ToHit
            w.WriteInt("Damages", attack.Damages.Count); //write # damages in Damages
            foreach (var damage in attack.Damages) //for each damage in Damages
            {
                w.WriteSingle(damage); //write it
            }
            w.WriteInt("DamageTypes", attack.DamageTypes.Count); //write # damagetypes in DamageTypes
            foreach (var dtype in attack.DamageTypes) //for each dtype in DamageTypes
            {
                w.WriteSingle(Writer.DamageTypeToWord(dtype)); //write word for dtype
            }
            w.WriteWords("Effect", Writer.EffectToWord(attack.Effect)); //write Effect as word
            w.WriteInt("Range", attack.Range); //write Range
            w.WriteInt("MaxRange", attack.MaxRange); //write MaxRange
            w.WriteInt("Area", attack.Area); //write Area
            w.WriteSingle("end"); //write end
        }
        public static void Load(Attack attack, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get new Reader
            while (word1 != "end") //while we haven't reached the end
            {
                (word1, word2) = r.ParseToWords(); //get words from current line
                switch (word1) //switch on first word (which is changed to lowercase)
                {
                    case "name": //if name
                        if (word2 != null) //if we have a name
                        {
                            attack.Name = word2; //set Name
                        }
                        break;
                    case "tohit": //if tohit
                    case "to hit": //or to hit
                        attack.ToHit = Reader.WordToInt(word2); //read ToHit
                        break;
                    case "damages": //if damages
                        int n = Reader.WordToInt(word2); //get number values
                        attack.Damages.Clear(); //clear Damages
                        for (int i = 0; i < n; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get single word
                            if (word2 != null) //if it is not null
                            {
                                attack.Damages.Add(word2); //add it to Damages
                            }
                        }
                        break;
                    case "damagetypes": //if damagetypes
                    case "damage types": //or damage types
                        int n2 = Reader.WordToInt(word2); //get number values
                        attack.DamageTypes.Clear(); //clear DamageTypes
                        for (int i = 0; i < n2; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get single word
                            if (word2 != null) //if it is not null
                            {
                                DamageType type = Reader.WordToDamageType(word2); //get damagetype from word
                                attack.DamageTypes.Add(type); //add it to DamageTypes
                            }
                        }
                        break;
                    case "effect": //if effect
                        attack.Effect = Reader.WordToEffect(word2); //get Effect
                        break;
                    case "range": //if range
                        attack.Range = Reader.WordToInt(word2); //get Range
                        break;
                    case "maxrange": //if maxrange
                        attack.MaxRange = Reader.WordToInt(word2); //get MaxRange
                        break;
                    case "area": //if area
                        attack.Area = Reader.WordToInt(word2); //get Area
                        break;
                    case "end": //if end
                    case null: //or if null
                        return; //return
                }
            }
        }
        public static void Save(Thing thing, StreamWriter sw)
        {
            Writer w = new(sw); //get new Writer
            w.WriteWords("Name", thing.Name); //write Name
            w.WriteInt("X", thing.X); //write X
            w.WriteInt("Y", thing.Y); //write Y
            w.WriteSpot(thing.Spot); //write Spot
            w.WriteWords("Description", thing.Description); //write Description
            w.WriteInt("Weight", thing.Weight); //write Weight
            w.WriteInt("Value", thing.Value); //write Value
            w.WriteWords("PotionType", Writer.PotionTypeToWord(thing.Potion)); //write Potion
            w.WriteWords("ScrollType", Writer.ScrollTypeToWord(thing.Scroll)); //write Scroll
            w.WriteBool("Consumable", thing.Consumable); //write Consumable
            w.WriteBool("Equippable", thing.Equippable); //write Equippable
            w.WriteBool("Equipped", thing.Equipped); //write Equipped
            w.WriteWords("Effect", Writer.EffectToWord(thing.Effect)); //write Effect
            w.WriteInt("Bonus", thing.Bonus); //write Bonus
            w.WriteInt("Armor", thing.Armor); //write Armor
            w.WriteInt("Damages", thing.Damages.Count); //write # values in Damages
            foreach (var dam in thing.Damages) //for each value in Damages
            {
                w.WriteSingle(dam); //write it
            }
            w.WriteInt("DamageTypes", thing.DamageTypes.Count); //write # values in DamageTypes
            foreach (var dtype in thing.DamageTypes) //for each value in DamageTypes
            {
                w.WriteSingle(Writer.DamageTypeToWord(dtype)); //write it as a word
            }
            w.WriteInt("Range", thing.Range); //write Range
            w.WriteInt("MaxRange", thing.MaxRange); //write MaxRange
            w.WriteInt("Area", thing.Area); //write Area
            w.WriteBool("Open", thing.Open); //write Open
            w.WriteBool("Locked", thing.Locked); //write Locked
            w.WriteInt("Things", thing.Things.Count); //write # values in Things
            foreach (var t in thing.Things) //for each value in Things
            {
                t.Save(sw); //save it
            }
            w.WriteWords("Alignment", Writer.AlignmentToWord(thing.Alignment));
            w.WriteBool("Dead", thing.Dead); //write Dead
            w.WriteBool("Hostile", thing.Hostile); //write Hostile
            w.WriteInt("HP", thing.HP); //write HP
            w.WriteInt("MaxHP", thing.MaxHP); //write MaxHP
            w.WriteInt("Speed", thing.Speed); //write Speed
            w.WriteInt("FlySpeed", thing.FlySpeed); //write FlySpeed
            w.WriteInt("ClimbSpeed", thing.ClimbSpeed); //write ClimbSpeed
            w.WriteInt("SwimSpeed", thing.SwimSpeed); //write SwimSpeed
            w.WriteInt("AC", thing.AC); //write AC
            w.WriteInt("STR", thing.STR); //write STR
            w.WriteInt("DEX", thing.DEX); //write DEX
            w.WriteInt("CON", thing.CON); //write CON
            w.WriteInt("INT", thing.INT); //write INT
            w.WriteInt("WIS", thing.WIS); //write WIS
            w.WriteInt("CHA", thing.CHA); //write CHA
            w.WriteWords("CreatureType", Writer.CreatureTypeToWord(thing.Creature)); //write Creature as word
            w.WriteInt("Experience", thing.Experience); //write Experience
            w.WriteInt("Level", thing.Level); //write Level
            w.WriteInt("NumberAttacks", thing.NumberAttacks); //write NumberAttacks
            w.WriteInt("Attacks", thing.Attacks.Count); //write # values in Attacks
            foreach (var att in thing.Attacks) //for each value in Attacks
            {
                att.Save(sw); //save it
            }
            w.WriteWords("ClassType", Writer.ClassTypeToWord(thing.Class)); //write Class as word
            w.WriteInt("Resistances", thing.Resistances.Count); //write # values in Resistances
            foreach (var r in thing.Resistances) //for each value in Resistances
            {
                w.WriteSingle(Writer.DamageTypeToWord(r)); //write it as a word
            }
            w.WriteInt("Immunities", thing.Immunities.Count); //write # values in Immunities
            foreach (var i in thing.Immunities) //for each value in Immunities
            {
                w.WriteSingle(Writer.DamageTypeToWord(i)); //write it as a word
            }
            w.WriteInt("Vulnerabilities", thing.Vulnerabilities.Count); //write # values in Vulnerabilities
            foreach (var v in thing.Vulnerabilities) //for each value in Vulnerabilities
            {
                w.WriteSingle(Writer.DamageTypeToWord(v)); //write it as a word
            }
            w.WriteInt("LightRadius", thing.LightRadius); //write LightRadius
            w.WriteWords("RaceType", Writer.RaceTypeToWord(thing.BaseRace)); //write BaseRace as word
            w.WriteWords("Race", thing.Race); //write Race
            w.WriteWords("MonsterType", Writer.MonsterTypeToWord(thing.Monster)); //write Monster as word
            w.WriteInt("SpellList", thing.SpellList.Count); //write # values in SpellList
            foreach (var s in thing.SpellList) //for each spell in SpellList
            {
                s.Save(sw); //save it
            }
            w.WriteInt("Condition", thing.Condition.Count); //write # values in Condition
            foreach (var c in thing.Condition) //for each condition in Condition
            {
                c.Save(sw); //save it
            }
            w.WriteSingle("end"); //write end
        }
        public static void Load(Thing thing, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get a new reader
            while (word1 != "end") //while first word isn't end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch (word1) //switch on first word (lowercased)
                {
                    case "name": //if name
                        if (word2 != null) //and second word isn't null
                        {
                            thing.Name = word2; //set Name
                        }
                        break;
                    case "x": //if x
                        thing.X = Reader.WordToInt(word2); //set X
                        break;
                    case "y": //if y
                        thing.Y = Reader.WordToInt(word2); //set Y
                        break;
                    case "spot": //if spot
                        thing.Spot = r.ReadSpot(); //read Spot
                        break;
                    case "description": //if description
                    case "desc.": //or desc.
                    case "desc": //or desc
                        if (word2 != null) //if second word
                        {
                            thing.Description = word2; //set Description
                        }
                        break;
                    case "weight": //if weight
                        thing.Weight = Reader.WordToInt(word2); //set Weight
                        break;
                    case "value": //if value
                        thing.Value = Reader.WordToInt(word2); //set Value
                        break;
                    case "potiontype": //if potiontype
                    case "potion type": //or potion type
                    case "potion": //or even potion
                        thing.Potion = Reader.WordToPotion(word2); //set Potion
                        break;
                    case "scrolltype": //if scrolltype
                    case "scroll type": //or scroll type
                    case "scroll": //or even scroll
                        thing.Scroll = Reader.WordToScroll(word2); //set Scroll
                        break;
                    case "consumable": //if consumable
                        thing.Consumable = Reader.WordToBool(word2); //set Consumable
                        break;
                    case "equippable": //if equippable
                        thing.Equippable = Reader.WordToBool(word2); //set Equippable
                        break;
                    case "equipped": //if equipped
                        thing.Equipped = Reader.WordToBool(word2); //set Equipped
                        break;
                    case "effect": //if effect
                        thing.Effect = Reader.WordToEffect(word2); //set Effect
                        break;
                    case "bonus": //if bonus
                        thing.Bonus = Reader.WordToInt(word2); //set Bonus
                        break;
                    case "armor": //if armor
                        thing.Armor = Reader.WordToInt(word2); //set Armor
                        break;
                    case "damages": //if damages
                    case "damage": //or damage
                        int nd = Reader.WordToInt(word2); //get # values
                        thing.Damages.Clear(); //clear Damages
                        for (int i = 0; i < nd; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get a word
                            if (word2 != null) //if it is not null
                            {
                                thing.Damages.Add(word2); //add to Damages
                            }
                        }
                        break;
                    case "damagetypes": //if damagetypes
                    case "damage types": //or damage types
                    case "damagetype": //or damagetype
                    case "damage type": //or damage type
                        int nt = Reader.WordToInt(word2); //get # values
                        thing.DamageTypes.Clear(); //clear DamageTypes
                        for (int i = 0; i < nt; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get a word
                            if (word2 != null) //if it is not null
                            {
                                thing.DamageTypes.Add(Reader.WordToDamageType(word2)); //add (translated) to DamageTypes
                            }
                        }
                        break;
                    case "range": //if range
                        thing.Range = Reader.WordToInt(word2); //set Range
                        break;
                    case "maxrange": //if maxrange
                    case "maximumrange":
                    case "max range":
                    case "maximum range":
                        thing.MaxRange = Reader.WordToInt(word2); //set MaxRange
                        break;
                    case "area": //if area
                        thing.Area = Reader.WordToInt(word2); //set Area
                        break;
                    case "open": //if open
                        thing.Open = Reader.WordToBool(word2); //set Open
                        break;
                    case "locked": //if locked
                    case "lock": //or lock
                        thing.Locked = Reader.WordToBool(word2); //set Locked
                        break;
                    case "things": //if things
                    case "thing": //or thing
                    case "inventory": //or inventory
                        int n = Reader.WordToInt(word2); //get # values
                        thing.Things.Clear(); //clear Things
                        for (int i = 0; i < n; i++) //for each value
                        {
                            Thing t = new(sr); //make a new Thing
                            thing.Things.Add(t); //add it to Things
                        }
                        break;
                    case "alignment": //if alignment
                        thing.Alignment = Reader.WordToAlignment(word2); //set alignment
                        break;
                    case "dead": //if dead
                        thing.Dead = Reader.WordToBool(word2); //set Dead
                        break;
                    case "hostile": //if hostile
                        thing.Hostile = Reader.WordToBool(word2); //set Hostile
                        break;
                    case "hp": //if hp
                    case "hitpoints": //or hitpoints
                    case "hit points": //or hit points
                        thing.HP = Reader.WordToInt(word2); //set HP
                        break;
                    case "maxhp": //case maxhp
                    case "max hp": //or max hp
                    case "hpmax": //or hpmax
                        thing.MaxHP = Reader.WordToInt(word2); //set MaxHP
                        break;
                    case "speed": //case speed
                        thing.Speed = Reader.WordToInt(word2); //set Speed
                        break;
                    case "flyspeed": //case flyspeed
                    case "fly speed": //or fly speed
                    case "fly": //or even fly
                        thing.FlySpeed = Reader.WordToInt(word2); //set FlySpeed
                        break;
                    case "climbspeed": //case climbspeed
                    case "climb speed": //or climb speed
                    case "climb": //or even climb
                        thing.ClimbSpeed = Reader.WordToInt(word2); //set ClimbSpeed
                        break;
                    case "swimspeed": //case swimspeed
                    case "swim speed": //or swim speed
                    case "swim": //or even swim
                        thing.SwimSpeed = Reader.WordToInt(word2); //set SwimSpeed
                        break;
                    case "ac": //case ac
                    case "armorclass": //or armorclass
                    case "armor class": //or armor class
                        thing.AC = Reader.WordToInt(word2); //set AC
                        break;
                    case "str": //case str
                    case "strength": //or strength
                        thing.STR = Reader.WordToInt(word2); //set STR
                        break;
                    case "dex": //case dex
                    case "dexterity": //or dexterity
                        thing.DEX = Reader.WordToInt(word2); //set DEX
                        break;
                    case "con": //case con
                    case "constitution": //or constitution
                        thing.CON = Reader.WordToInt(word2); //set CON
                        break;
                    case "int": //case int
                    case "intelligence": //or intelligence
                        thing.INT = Reader.WordToInt(word2); //set INT
                        break;
                    case "wis": //case wis
                    case "wisdom": //or wisdom
                        thing.WIS = Reader.WordToInt(word2); //set WIS
                        break;
                    case "cha": //case cha
                    case "charisma": //or charisma
                        thing.CHA = Reader.WordToInt(word2); //set CHA
                        break;
                    case "creaturetype": //case creaturetype
                    case "creature type": //or creature type
                    case "creature": //or even creature
                        thing.Creature = Reader.WordToCreatureType(word2); //set Creature
                        break;
                    case "experience": //case experience
                    case "exp": //or exp
                    case "xp": //or xp
                        thing.Experience = Reader.WordToInt(word2); //set Experience
                        break;
                    case "level": //case level
                        thing.Level = Reader.WordToInt(word2); //set Level
                        break;
                    case "numberattacks": //case numberattacks
                    case "numattacks": //or numattacks
                    case "#attacks": //or even #attacks
                        thing.NumberAttacks = Reader.WordToInt(word2); //set NumberAttacks
                        break;
                    case "attacks": //case attacks
                    case "attack": //or attack
                        int na = Reader.WordToInt(word2); //get # values
                        thing.Attacks.Clear(); //clear Attacks
                        for (int i = 0; i < na; i++) //for each value
                        {
                            Attack a = new(sr); //get new Attack
                            thing.Attacks.Add(a); //add to Attacks
                        }
                        break;
                    case "class": //case class
                        thing.Class = Reader.WordToClassType(word2); //set Class
                        break;
                    case "resistances": //case resistances
                    case "resistance": //case resistance
                    case "resist": //or even resist
                        int nr = Reader.WordToInt(word2); //get # values
                        thing.Resistances.Clear(); //clear Resistances
                        for (int i = 0; i < nr; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get word
                            if (word2 != null) //if not null
                            {
                                thing.Resistances.Add(Reader.WordToDamageType(word2)); //add (converted) to Resistances
                            }
                        }
                        break;
                    case "immunities": //case immunities
                    case "immunity": //or immunity
                    case "immune": //or even immune
                        int ni = Reader.WordToInt(word2); //get # values
                        thing.Immunities.Clear(); //clear Immunities
                        for (int i = 0; i < ni; i++) ///for each value
                        {
                            word2 = r.ParseSingle(); //get word
                            if (word2 != null) //if not null
                            {
                                thing.Immunities.Add(Reader.WordToDamageType(word2)); //add (converted) to Immunities
                            }
                        }
                        break;
                    case "vulnerabilities": //case vulnerabilities
                    case "vulnerability": //or vulnerablity
                    case "vulnerable": //or even vulnerable
                        int nv = Reader.WordToInt(word2); //get # values
                        thing.Vulnerabilities.Clear(); //clear Vulnerabilities
                        for (int i = 0; i < nv; i++) //for each value
                        {
                            word2 = r.ParseSingle(); //get word
                            if (word2 != null) //if not null
                            {
                                thing.Vulnerabilities.Add(Reader.WordToDamageType(word2)); //add (converted) to Vulnerabilities
                            }
                        }
                        break;
                    case "lightradius": //case lightradius
                    case "light radius": //or light radius
                    case "light": //or even light
                        thing.LightRadius = Reader.WordToInt(word2); //set LightRadius
                        break;
                    case "racetype": //case racetype
                    case "race type": //or race type
                    case "baserace": //or baserace
                    case "base race": //or base race
                        thing.BaseRace = Reader.WordToRaceType(word2); //set BaseRace
                        break;
                    case "race": //case race
                        if (word2 != null) //if word isn't null
                        {
                            thing.Race = word2; //set Race
                        }
                        break;
                    case "monstertype": //case monstertype
                    case "monster type": //or monseter type
                    case "monster": //or even monster
                        thing.Monster = Reader.WordToMonsterType(word2); //set Monster
                        break;
                    case "spelllist": //if spelllist
                    case "spells": //or spells
                        int ns = Reader.WordToInt(word2); //get # values
                        thing.Damages.Clear(); //clear Damages
                        for (int i = 0; i < ns; i++) //for each value
                        {
                            thing.SpellList.Add(new(sr)); //add new spell to spelllist
                        }
                        break;
                    case "condition": //case condition
                    case "conditions": //or conditions
                        int nc = Reader.WordToInt(word2); //get # values
                        thing.Condition.Clear(); //clear Condition
                        for (int i = 0; i < nc; i++) //for each value
                        {
                            thing.Condition.Add(new(sr)); //add new condition to condition
                        }
                        break;
                    case "end": //case end
                    case null: //or null
                        return; //return
                }
            }
        }
        public static void Save(Spell spell, StreamWriter sw)
        {
            Writer w = new(sw); //get new writer
            w.WriteWords("Name", spell.Name); //write Name
            w.WriteInt("Level", spell.Level); //write Level
            w.WriteInt("Range", spell.Range); //write Range
            w.WriteInt("Area", spell.Area); //write Area
            w.WriteInt("Duration", spell.Duration); //write Duration
            w.WriteBool("IsAttack", spell.IsAttack); //write IsAttack
            w.WriteInt("SpellcastingBonus", spell.SpellcastingBonus); //write SpellcastingBonus
            w.WriteInt("ToHit", spell.ToHit); //write ToHit
            w.WriteInt("SaveDC", spell.SaveDC); //write SaveDC
            w.WriteWords("Ability", Writer.AbilityTypeToWord(spell.Ability)); //write Ability
            w.WriteWords("Spell", Writer.SpellTypeToWord(spell.Effect)); //write Spell
            w.WriteInt("Classes", spell.Classes.Count); //write # of Classes
            foreach (var c in spell.Classes)
            {
                w.WriteSingle(Writer.ClassTypeToWord(c));
            }
        }
        public static void Load(Spell spell, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get new reader
            while (word1 != "end") //while first word isn't end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch (word1) //switch on first word
                {
                    case "name": //if name
                        if (word2 != null) //if there is one
                        {
                            spell.Name = word2; //set Name
                        }
                        break;
                    case "level": //if level
                        spell.Level = Reader.WordToInt(word2); //set Level
                        break;
                    case "range": //if range
                        spell.Range = Reader.WordToInt(word2); //set Range
                        break;
                    case "area": //if area
                        spell.Area = Reader.WordToInt(word2); //set Area
                        break;
                    case "duration": //if duration
                        spell.Duration = Reader.WordToInt(word2); //set Duration
                        break;
                    case "isattack": //if isattack
                        spell.IsAttack = Reader.WordToBool(word2); //set IsAttack
                        break;
                    case "spellcastingbonus": //if spellcastingbonus
                    case "spellbonus": //or spellbonus
                    case "castingbonus": //or castingbonus
                        spell.SpellcastingBonus = Reader.WordToInt(word2); //set SpellcastingBonus
                        break;
                    case "tohit": //if tohit
                        spell.ToHit = Reader.WordToInt(word2); //set ToHit
                        break;
                    case "savedc": //if savedc
                        spell.SaveDC = Reader.WordToInt(word2); //set SaveDC
                        break;
                    case "ability": //if ability
                        spell.Ability = Reader.WordToAbilityType(word2); //set Ability
                        break;
                    case "spell": //if spell
                        spell.Effect = Reader.WordToSpellType(word2); //set Spell
                        break;
                    case "classes": //if classes
                        int n = Reader.WordToInt(word2);
                        spell.Classes.Clear();
                        for (int i = 0; i < n; i++)
                        {
                            word2 = r.ParseSingle();
                            spell.Classes.Add(Reader.WordToClassType(word2));
                        }
                        break;
                    case "end": //if end
                    case null: //if null
                        return; //return
                }
            }
        }
        public static void Save(SpellEffect se, StreamWriter sw)
        {
            Writer w = new(sw); //get a new writer
            w.WriteWords("Effect", Writer.EffectToWord(se.Effect)); //write Effect
            w.WriteWords("Spell", Writer.SpellTypeToWord(se.Spell)); //write Spell
            w.WriteInt("Timeleft", se.Timeleft); //write Timeleft
            w.WriteInt("SaveDC", se.SaveDC); //write SaveDC
            w.WriteWords("Ability", Writer.AbilityTypeToWord(se.Ability)); //write Ability
            w.WriteSingle("end"); //write end
        }
        public static void Load(SpellEffect se, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get a new reader
            while (word1 != "end") //while first word isn't end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch (word1) //switch on first word
                {
                    case "effect": //if effect
                        se.Effect = Reader.WordToEffect(word2); //get Effect
                        break;
                    case "spell": //if spell
                        se.Spell = Reader.WordToSpellType(word2); //get Spell
                        break;
                    case "timeleft": //if timeleft
                    case "duration": //if duration
                    case "time": //if time
                        se.Timeleft = Reader.WordToInt(word2); //get Timeleft
                        break;
                    case "savedc": //if savedc
                    case "save": //or save
                    case "dc": //or even dc
                        se.SaveDC = Reader.WordToInt(word2); //get SaveDC
                        break;
                    case "ability": //if ability
                    case "stat": //or even stat
                        se.Ability = Reader.WordToAbilityType(word2); //get Ability
                        break;
                    case "end": //if end
                    case null: //if null
                        return; //we are done
                }
            }
        }

        public static void Save(Room room, StreamWriter sw)
        {
            Writer w = new(sw); //get a new writer
            w.WriteInt("X", room.X); //write X
            w.WriteInt("Y", room.Y); //write Y
            w.WriteInt("Wide", room.Wide); //write Wide
            w.WriteInt("High", room.High); //write High
            w.WriteWords("Type", Writer.RoomTypeToWord(room.Type));
            w.WriteInt("Spots", room.Spots.Count); //write # values in Spots
            foreach (var spot in room.Spots) //for each value in Spots
            {
                w.WriteSpot(spot); //write it
            }
            w.WriteInt("SpotPoints", room.SpotPoints.Count); //write # values in SpotPoints
            foreach (var point in room.SpotPoints) //for each value in SpotPoints
            {
                w.WriteInt("X", point.X); //write X
                w.WriteInt("Y", point.Y); //write Y
            }
            w.WriteSingle("end"); //write end
        }
        public static void Load(Room room, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get new reader
            while (word1 != "end") //if first word isn't end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch(word1) //switch on first word
                {
                    case "x": //case x
                        room.X = Reader.WordToInt(word2); //set X
                        break;
                    case "y": //case y
                        room.Y = Reader.WordToInt(word2); //set Y
                        break;
                    case "wide": //case wide
                        room.Wide = Reader.WordToInt(word2); //set Wide
                        break;
                    case "high": //case high
                        room.High = Reader.WordToInt(word2); //set High
                        break;
                    case "type": //case type
                        room.Type = Reader.WordToRoomType(word2); //set Type
                        break;
                    case "spots": //case spots
                        int sn = Reader.WordToInt(word2); //get # values
                        room.Spots.Clear(); //clear Spots
                        for (int i = 0; i < sn; i++) //for each value
                        {
                            word1 = r.ParseSingle(); //get word(s) ("Spot:")
                            Spot s = r.ReadSpot(); //read spot
                            room.Spots.Add(s); //add it to Spots
                        }
                        break;
                    case "spotpoints": //case spotpoints
                        int pn = Reader.WordToInt(word2); //get # values
                        room.SpotPoints.Clear(); //clear SpotPoints
                        for (int i = 0; i < pn; i++) //for each value
                        {
                            Point p = new(); //make a new point
                            for (int j = 0; j < 2; j++) //for 2 times
                            {
                                (word1, word2) = r.ParseToWords(); //get words
                                if (word1 == "x") //if first word is x
                                {
                                    p.X = Reader.WordToInt(word2); //set X
                                }
                                if (word1 == "y") //if first word is y
                                {
                                    p.Y = Reader.WordToInt(word2); //set Y
                                }
                            }
                            room.SpotPoints.Add(p); //add point to SpotPoints
                        }
                        break;
                    case "end": //case end
                    case null: //case null
                        return; //return
                }
            }
        }
        public static void Save(Door door, StreamWriter sw)
        {
            Writer w = new(sw); //get new writer
            w.WriteInt("X", door.X); //write X
            w.WriteInt("Y", door.Y); //write Y
            w.WriteSingle("Room:"); //write Room (header)
            Save(door.Room, sw); //save Room
            w.WriteSpot(door.Spot); //write Spot
            w.WriteSingle("end"); //write end
        }
        public static void Load(Door door, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get new reader
            while (word1 != "end") //while first word is not end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch (word1) //switch on first word
                {
                    case "x": //case x
                        door.X = Reader.WordToInt(word2); //set X
                        break;
                    case "y": //case y
                        door.Y = Reader.WordToInt(word2); //set Y
                        break;
                    case "room": //case room
                        Load(door.Room, sr); //load Room
                        break;
                    case "spot": //case spot
                        door.Spot = r.ReadSpot(); //set Spot
                        break;
                    case "end": //case end
                    case null: //case null
                        return; //return
                }
            }
        }
        public static void Save(Path path, StreamWriter sw)
        {
            Writer w = new(sw); //get new writer
            w.WriteInt("X", path.X); //write X
            w.WriteInt("Y", path.Y); //write Y
            w.WriteSpot(path.Spot); //write Spot
            w.WriteSingle("end"); //write end
        }
        public static void Load(Path path, StreamReader sr)
        {
            string? word1 = string.Empty, word2; //variables for words
            Reader r = new(sr); //get new reader
            while (word1 != "end") //while first word is not end
            {
                (word1, word2) = r.ParseToWords(); //get words
                switch(word1) //switch on first word
                {
                    case "x": //case x
                        path.X = Reader.WordToInt(word2); //set X
                        break;
                    case "y": //case y
                        path.Y = Reader.WordToInt(word2); //set Y
                        break;
                    case "spot": //case spot
                        path.Spot = r.ReadSpot(); //set Spot
                        break;
                    case "end": //case end
                    case null: //case null
                        return; //return
                }
            }
        }
        public static void Save(Level level, StreamWriter sw)
        {
            Writer w = new(sw);
            w.WriteInt("Rooms", level.Rooms.Count);
            foreach (var room in level.Rooms)
            {
                room.Save(sw);
            }
            w.WriteInt("Doors", level.Doors.Count);
            foreach (var door in level.Doors)
            {
                door.Save(sw);
            }
            w.WriteInt("Paths", level.Paths.Count);
            foreach (var path in level.Paths)
            {
                path.Save(sw);
            }
            w.WriteInt("Items", level.Items.Count);
            foreach (var item in level.Items)
            {
                item.Save(sw);
            }
            w.WriteInt("Creatures", level.Creatures.Count);
            foreach (var creature in level.Creatures)
            {
                creature.Save(sw);
            }
            w.WriteSingle("end");
        }
        public static void Load(Level level, StreamReader sr)
        {
            string? word1 = string.Empty, word2;
            Reader r = new(sr);
            while (word1 != "end")
            {
                (word1, word2) = r.ParseToWords();
                switch (word1)
                {
                    case "rooms":
                        int rn = Reader.WordToInt(word2);
                        level.Rooms.Clear();
                        for (int i = 0; i < rn; i++)
                        {
                            level.Rooms.Add(new(sr));
                        }
                        break;
                    case "doors":
                        int dn = Reader.WordToInt(word2);
                        level.Doors.Clear();
                        for (int i = 0; i < dn; i++)
                        {
                            level.Doors.Add(new(sr));
                        }
                        break;
                    case "paths":
                        int pn = Reader.WordToInt(word2);
                        level.Paths.Clear();
                        for (int i = 0; i < pn; i++)
                        {
                            level.Paths.Add(new(sr));
                        }
                        break;
                    case "items":
                        int ni = Reader.WordToInt(word2);
                        level.Items.Clear();
                        for (int i = 0; i < ni; i++)
                        {
                            level.Items.Add(new(sr));
                        }
                        break;
                    case "creatures":
                        int nc = Reader.WordToInt(word2);
                        level.Creatures.Clear();
                        for (int i = 0; i < nc; i++)
                        {
                            level.Creatures.Add(new(sr));
                        }
                        break;
                    case "end":
                    case null:
                        return;
                }
            }
        }
    }
    public class Reader
    {
        readonly StreamReader sr;
        public Reader(StreamReader sr) => this.sr = sr;
        public (string?, string?) ParseToWords()
        {
            string? input, word1, word2; //variables
            input = sr.ReadLine()?.Trim(); //read in a line and trim off whitespace
            if (input == null) //if we got nothing
            {
                return (null, null); //return that
            }
            if (input.Contains(':')) //if input contains a :
            {
                word1 = input[..input.IndexOf(':')].ToLower(); //set first word to input up to the : and make lowercase
                word2 = input[(input.IndexOf(':') + 1)..].Trim(); //set second word to after the colon and trim whitespace
                return (word1, word2); //return both words
            }
            return (input.ToLower(), null); //otherwise return the input made lowercase
        }
        public string? ParseSingle()
        {
            string? input, word; //variables
            input = sr.ReadLine()?.Trim(); //read in a line and trim off whitespace
            if (input == null) //if we got nothing
            {
                return null; //return that
            }
            word = input.ToLower(); //make lowercase
            return word; //return it
        }
        public static int WordToInt(string? word)
        {
            if (word == null) //if word is null
            {
                return -1; //return -1
            }
            _ = int.TryParse(word, out int r); //parse the word into an integer
            return r; //return the integer
        }
        public static SpellType WordToSpellType(string? word)
        {
            return DoWord(word) switch //parse word into a SpellType
            {
                "healing" => SpellType.Healing,
                "extrahealing" => SpellType.ExtraHealing,
                "invisibility" => SpellType.Invisibility,
                "magicmapping" => SpellType.MagicMapping,
                "slowmonsters" => SpellType.SlowMonsters,
                "slow" => SpellType.SlowMonsters,
                "sleepmonsters" => SpellType.SleepMonsters,
                "sleep" => SpellType.SleepMonsters,
                "confusemonsters" => SpellType.ConfuseMonsters,
                "confusion" => SpellType.ConfuseMonsters,
                "confuse" => SpellType.ConfuseMonsters,
                "acidsplash" => SpellType.AcidSplash,
                "chilltouch" => SpellType.ChillTouch,
                "firebolt" => SpellType.FireBolt,
                "friends" => SpellType.Friends,
                "light" => SpellType.Light,
                "poisonspray" => SpellType.PoisonSpray,
                "rayoffrost" => SpellType.RayOfFrost,
                "shockinggrasp" => SpellType.ShockingGrasp,
                "burninghands" => SpellType.BurningHands,
                "magearmor" => SpellType.MageArmor,
                "magicmissile" => SpellType.MagicMissile,
                "tashashideouslaughter" => SpellType.TashasHideousLaughter,
                "tashas" => SpellType.TashasHideousLaughter,
                "hideouslaughter" => SpellType.TashasHideousLaughter,
                "laughter" => SpellType.TashasHideousLaughter,
                "witchbolt" => SpellType.WitchBolt,
                _ => SpellType.None,
            };
        }
        public static DamageType WordToDamageType(string? word)
        {
            return DoWord(word) switch //parse word into a DamageType
            {
                "acid" => DamageType.Acid,
                "fire" => DamageType.Fire,
                "cold" => DamageType.Cold,
                "electricity" => DamageType.Electricity,
                "lightning" => DamageType.Electricity, //added in case
                "slashing" => DamageType.Slashing,
                "piercing" => DamageType.Piercing,
                "bludgeoning" => DamageType.Bludgeoning,
                "necrotic" => DamageType.Necrotic,
                "poison" => DamageType.Poison,
                "force" => DamageType.Force,
                _ => DamageType.None,
            };
        }
        public static Alignment WordToAlignment(string? word)
        {
            return DoWord(word) switch //parse word into an Alignment
            {
                "none" => Alignment.None,
                "chaoticgood" => Alignment.ChaoticGood,
                "cg" => Alignment.ChaoticGood,
                "chaoticneutral" => Alignment.ChaoticNeutral,
                "cn" => Alignment.ChaoticNeutral,
                "chaoticevil" => Alignment.ChaoticEvil,
                "ce" => Alignment.ChaoticEvil,
                "neutralgood" => Alignment.NeutralGood,
                "ng" => Alignment.NeutralGood,
                "trueneutral" => Alignment.TrueNeutral,
                "neutralneutral" => Alignment.TrueNeutral,
                "tn" => Alignment.TrueNeutral,
                "nn" => Alignment.TrueNeutral,
                "neutralevil" => Alignment.NeutralEvil,
                "ne" => Alignment.NeutralEvil,
                "lawfulgood" => Alignment.LawfulGood,
                "lg" => Alignment.LawfulGood,
                "lawfulneutral" => Alignment.LawfulNeutral,
                "ln" => Alignment.LawfulNeutral,
                "lawfulevil" => Alignment.LawfulEvil,
                "le" => Alignment.LawfulEvil,
                _ => Alignment.None,
            };
        }
        public static Effect WordToEffect(string? word)
        {
            return DoWord(word) switch //parse word into an Effect
            {
                "confusion" => Effect.Confusion,
                "sleep" => Effect.Sleep,
                "slow" => Effect.Slow,
                "speed" => Effect.Speed,
                "blind" => Effect.Blind,
                "charmed" => Effect.Charmed,
                "deafened" => Effect.Deafened,
                "frightened" => Effect.Frightened,
                "grappled" => Effect.Grappled,
                "incapacitated" => Effect.Incapacitated,
                "invisible" => Effect.Invisible,
                "paralyzed" => Effect.Paralyzed,
                "petrified" => Effect.Petrified,
                "poisoned" => Effect.Poisoned,
                "prone" => Effect.Prone,
                "restrained" => Effect.Restrained,
                "stunned" => Effect.Stunned,
                "unconscious" => Effect.Unconscious,
                _ => Effect.None,
            };
        }
        public Spot ReadSpot()
        {
            string? word1, word2; //variables
            Spot spot = new(); //make a new spot
            for (int i = 0; i < 2; i++) //we will have two records
            {
                (word1, word2) = ParseToWords(); //get words from input
                switch (word1) //switch on first word
                {
                    case "type": //if type
                        spot.Type = WordToSpotType(word2); //set spot.Type from translated word
                        break;
                    case "seen": //if seen
                        spot.Seen = WordToSeenType(word2); //set spot.Seen from translated word
                        break;
                }
            }
            return spot; //return spot
        }
        public static SpotType WordToSpotType(string? word)
        {
            return DoWord(word) switch //parse word into SpotType
            {
                "player" => SpotType.Player,
                "animal" => SpotType.Animal,
                "creature" => SpotType.Creature, //.Animal and .Creature are the same
                "monster" => SpotType.Monster,
                "npc" => SpotType.NPC,
                "armor" => SpotType.Armor,
                "weapon" => SpotType.Weapon,
                "potion" => SpotType.Potion,
                "scroll" => SpotType.Scroll,
                "gold" => SpotType.Gold,
                "lightsource" => SpotType.LightSource,
                "light source" => SpotType.LightSource, //just in case
                "light" => SpotType.LightSource, //just in case
                "room" => SpotType.Room,
                "wall" => SpotType.Wall,
                "path" => SpotType.Path,
                "door" => SpotType.Door,
                "stairsdown" => SpotType.StairsDown,
                "stairs down" => SpotType.StairsDown, //just in case
                "stairs" => SpotType.StairsDown, //just in case
                "stairsup" => SpotType.StairsUp,
                "stairs up" => SpotType.StairsUp, //just in case
                "solid" => SpotType.Solid,
                "statue" => SpotType.Solid, //just in case
                "column" => SpotType.Solid, //just in case
                "takable" => SpotType.Takable,
                _ => SpotType.None,
            };
        }
        public static SeenType WordToSeenType(string? word)
        {
            return DoWord(word) switch //parse word into SeenType
            {
                "hidden" => SeenType.Hidden,
                "notseen" => SeenType.NotSeen,
                "seen" => SeenType.Seen,
                "remembered" => SeenType.Remembered,
                "recalled" => SeenType.Remembered, //just in case
                _ => SeenType.NotSeen,
            };
        }
        public static PotionType WordToPotion(string? word)
        {
            return DoWord(word) switch //parse word into PotionType
            {
                "healing" => PotionType.Healing,
                "invisibility" => PotionType.Invisibility,
                "extrahealing" => PotionType.ExtraHealing,
                "extra healing" => PotionType.ExtraHealing, //just in case
                _ => PotionType.None,
            };
        }
        public static ScrollType WordToScroll(string? word)
        {
            return DoWord(word) switch //parse word into ScrollType
            {
                "magicmapping" => ScrollType.MagicMapping,
                "magic mapping" => ScrollType.MagicMapping, //just in case
                "mapping" => ScrollType.MagicMapping, //just in case
                "confusion" => ScrollType.Confusion,
                "sleep" => ScrollType.Sleep,
                _ => ScrollType.None,
            };
        }
        public static CreatureType WordToCreatureType(string? word)
        {
            return DoWord(word) switch //parse word into CreatureType
            {
                "animal" => CreatureType.Animal,
                "monster" => CreatureType.Monster,
                "player" => CreatureType.Player,
                "npc" => CreatureType.NPC,
                _ => CreatureType.None,
            };
        }
        public static ClassType WordToClassType(string? word)
        {
            return DoWord(word) switch //parse word into ClassType
            {
                "artificer" => ClassType.Artificer,
                "barbarian" => ClassType.Barbarian,
                "bard" => ClassType.Bard,
                "cleric" => ClassType.Cleric,
                "druid" => ClassType.Druid,
                "fighter" => ClassType.Fighter,
                "monk" => ClassType.Monk,
                "paladin" => ClassType.Paladin,
                "ranger" => ClassType.Ranger,
                "rogue" => ClassType.Rogue,
                "sorcerer" => ClassType.Sorcerer,
                "warlock" => ClassType.Warlock,
                "wizard" => ClassType.Wizard,
                "explorer" => ClassType.Explorer,
                _ => ClassType.None,
            };
        }
        public static RaceType WordToRaceType(string? word)
        {
            return DoWord(word) switch //parse word into RaceType
            {
                "dragonborn" => RaceType.Dragonborn,
                "drow" => RaceType.DrowElf,
                "drowelf" => RaceType.DrowElf,
                "elf, drow" => RaceType.DrowElf,
                "dwarf" => RaceType.HillDwarf,
                "hilldwarf" => RaceType.HillDwarf,
                "mountaindwarf" => RaceType.MountainDwarf,
                "elf" => RaceType.HighElf,
                "highelf" => RaceType.HighElf,
                "woodelf" => RaceType.WoodElf,
                "gnome" => RaceType.ForestGnome,
                "forestgnome" => RaceType.ForestGnome,
                "rockgnome" => RaceType.RockGnome,
                "halfelf" => RaceType.HalfElf,
                "halfling" => RaceType.LightfootHalfling,
                "lightfoothalfling" => RaceType.LightfootHalfling,
                "stouthalfling" => RaceType.StoutHalfling,
                "halforc" => RaceType.HalfOrc,
                "human" => RaceType.Human,
                "tiefling" => RaceType.Tiefling,
                _ => RaceType.Other,
            };
        }
        public static MonsterType WordToMonsterType(string? word)
        {
            return DoWord(word) switch //parse word into MonsterType
            {
                "kobold-orc" => MonsterType.Kobold_Orc,
                "kobold to orc" => MonsterType.Kobold_Orc, //just in case
                "kobold" => MonsterType.Kobold_Orc, //just in case
                "goblin" => MonsterType.Kobold_Orc, //just in case
                "orc" => MonsterType.Kobold_Orc, //just in case
                _ => MonsterType.None,
            };
        }
        public static AbilityType WordToAbilityType(string? word)
        {
            return DoWord(word) switch //parse word into AbilityType
            {
                "str" => AbilityType.STR,
                "dex" => AbilityType.DEX,
                "con" => AbilityType.CON,
                "int" => AbilityType.INT,
                "wis" => AbilityType.WIS,
                "cha" => AbilityType.CHA,
                _ => AbilityType.None
            };
        }
        public static bool WordToBool(string? word)
        {
            return DoWord(word) switch //parse word into boolean
            {
                "true" => true,
                "yes" => true, //just in case
                "1" => true, //just in case
                _ => false,
            };
        }
        public static RoomType WordToRoomType(string? word)
        {
            return DoWord(word) switch //parse word into RoomType
            {
                "normal" => RoomType.Normal,
                "safe" => RoomType.Safe,
                "lit" => RoomType.Lit,
                "shop" => RoomType.Shop,
                _ => RoomType.Normal,
            };
        }
        public static string? DoWord(string? word)
        {
            if (word == null)
            {
                return null;
            }
            return word.ToLower();
        }
    }
    public class Writer
    {
        readonly StreamWriter sw;
        public Writer(StreamWriter sw) => this.sw = sw;
        public void WriteWords(string word1, string word2)
        {
            sw.WriteLine($"{word1}: {word2}"); //write words
        }
        public void WriteInt(string word, int i)
        {
            sw.WriteLine($"{word}: {i}"); //write integer
        }
        public void WriteSingle(string word)
        {
            sw.WriteLine($"{word}"); //write single word
        }
        public static string SpellTypeToWord(SpellType spell)
        {
            return spell switch //parse SpellType to word
            {
                SpellType.Healing => "Healing",
                SpellType.ExtraHealing => "ExtraHealing",
                SpellType.Invisibility => "Invisibility",
                SpellType.MagicMapping => "MagicMapping",
                SpellType.SlowMonsters => "SlowMonsters",
                SpellType.SleepMonsters => "SleepMonsters",
                SpellType.ConfuseMonsters => "ConfuseMonsters",
                SpellType.AcidSplash => "AcidSplash",
                SpellType.ChillTouch => "ChillTouch",
                SpellType.FireBolt => "FireBolt",
                SpellType.Friends => "Friends",
                SpellType.Light => "Light",
                SpellType.PoisonSpray => "PoisonSpray",
                SpellType.RayOfFrost => "RayOfFrost",
                SpellType.ShockingGrasp => "ShockingGrasp",
                SpellType.BurningHands => "BurningHands",
                SpellType.MageArmor => "MageArmor",
                SpellType.MagicMissile => "MagicMissile",
                SpellType.TashasHideousLaughter => "TashasHideousLaughter",
                SpellType.WitchBolt => "WitchBolt",
                _ => "None",
            };
        }
        public static string AbilityTypeToWord(AbilityType ability)
        {
            return ability switch //parse AbilityType to word
            {
                AbilityType.STR => "STR",
                AbilityType.DEX => "DEX",
                AbilityType.CON => "CON",
                AbilityType.INT => "INT",
                AbilityType.WIS => "WIS",
                AbilityType.CHA => "CHA",
                _ => "none"
            };
        }
        public static string DamageTypeToWord(DamageType type)
        {
            return type switch //parse DamageType to word
            {
                DamageType.Acid => "acid",
                DamageType.Fire => "fire",
                DamageType.Cold => "cold",
                DamageType.Electricity => "electricity",
                DamageType.Slashing => "slashing",
                DamageType.Piercing => "piercing",
                DamageType.Bludgeoning => "bludgeoning",
                DamageType.Necrotic => "necrotic",
                DamageType.Poison => "poison",
                DamageType.Force => "force",
                _ => "none",
            };
        }
        public static string AlignmentToWord(Alignment alignment)
        {
            return alignment switch
            {
                Alignment.ChaoticGood => "ChaoticGood",
                Alignment.ChaoticNeutral => "ChaoticNeutral",
                Alignment.ChaoticEvil => "ChaoticEvil",
                Alignment.NeutralGood => "NeutralGood",
                Alignment.TrueNeutral => "TrueNeutral",
                Alignment.NeutralEvil => "NeutralEvil",
                Alignment.LawfulGood => "LawfulGood",
                Alignment.LawfulNeutral => "LawfulNeutral",
                Alignment.LawfulEvil => "LawfulEvil",
                _ => "None",
            };
        }
        public static string EffectToWord(Effect effect)
        {
            return effect switch //parse Effect to word
            {
                Effect.Sleep => "sleep",
                Effect.Confusion => "confusion",
                Effect.Slow => "slow",
                Effect.Speed => "speed",
                Effect.Blind => "blind",
                Effect.Charmed => "charmed",
                Effect.Deafened => "deafened",
                Effect.Frightened => "frightened",
                Effect.Grappled => "grappled",
                Effect.Incapacitated => "incapacitated",
                Effect.Invisible => "invisible",
                Effect.Paralyzed => "paralyzed",
                Effect.Petrified => "petrified",
                Effect.Poisoned => "poisoned",
                Effect.Prone => "prone",
                Effect.Restrained => "restrained",
                Effect.Stunned => "stunned",
                Effect.Unconscious => "unconscious",
                _ => "none",
            };
        }
        public void WriteSpot(Spot spot)
        {
            sw.WriteLine("Spot:"); //write Spot (header)
            sw.WriteLine($"Type: {SpotTypeToWord(spot.Type)}"); //write spot.Type (translated)
            sw.WriteLine($"Seen: {SeenTypeToWord(spot.Seen)}"); //write spot.Seen (translated)
        }
        public static string SeenTypeToWord(SeenType seen)
        {
            return seen switch //translate SeenType to word
            {
                SeenType.Hidden => "hidden",
                SeenType.Seen => "seen",
                SeenType.Remembered => "remembered",
                _ => "notseen",
            };
        }
        public static string SpotTypeToWord(SpotType type)
        {
            return type switch //translate SpotType to word
            {
                SpotType.Player => "player",
                SpotType.Animal => "animal", //SpotType.Creature also
                SpotType.Monster => "monster",
                SpotType.NPC => "npc",
                SpotType.Armor => "armor",
                SpotType.Weapon => "weapon",
                SpotType.Potion => "potion",
                SpotType.Scroll => "scroll",
                SpotType.Gold => "gold",
                SpotType.LightSource => "lightsource",
                SpotType.Room => "room",
                SpotType.Wall => "wall",
                SpotType.Path => "path",
                SpotType.Door => "door",
                SpotType.StairsDown => "stairsdown",
                SpotType.StairsUp => "stairsup",
                SpotType.Solid => "solid",
                SpotType.Takable => "takable",
                _ => "none",
            };
        }
        public static string PotionTypeToWord(PotionType type)
        {
            return type switch //translate PotionType to word
            {
                PotionType.Healing => "healing",
                PotionType.Invisibility => "invisibility",
                PotionType.ExtraHealing => "extrahealing",
                _ => "none",
            };
        }
        public static string ScrollTypeToWord(ScrollType type)
        {
            return type switch //translate ScrollType to word
            {
                ScrollType.Sleep => "sleep",
                ScrollType.Confusion => "confusion",
                ScrollType.MagicMapping => "magicmapping",
                _ => "none",
            };
        }
        public static string CreatureTypeToWord(CreatureType type)
        {
            return type switch //translate CreatureType to word
            {
                CreatureType.Player => "player",
                CreatureType.Monster => "monster",
                CreatureType.Animal => "animal",
                CreatureType.NPC => "npc",
                _ => "none",
            };
        }
        public static string ClassTypeToWord(ClassType type)
        {
            return type switch //translate ClassType to word
            {
                ClassType.Artificer => "artificer",
                ClassType.Barbarian => "barbarian",
                ClassType.Bard => "bard",
                ClassType.Cleric => "cleric",
                ClassType.Druid => "druid",
                ClassType.Fighter => "fighter",
                ClassType.Monk => "monk",
                ClassType.Paladin => "paladin",
                ClassType.Ranger => "ranger",
                ClassType.Rogue => "rogue",
                ClassType.Sorcerer => "sorcerer",
                ClassType.Warlock => "warlock",
                ClassType.Wizard => "wizard",
                ClassType.Explorer => "explorer",
                _ => "none",
            };
        }
        public static string RaceTypeToWord(RaceType race)
        {
            return race switch //translate RaceType to word
            {
                RaceType.Dragonborn => "dragonborn",
                RaceType.DrowElf => "drowelf",
                RaceType.HillDwarf => "hilldwarf",
                RaceType.MountainDwarf => "mountaindwarf",
                RaceType.HighElf => "highelf",
                RaceType.WoodElf => "woodelf",
                RaceType.ForestGnome => "forestgnome",
                RaceType.RockGnome => "rockgnome",
                RaceType.HalfElf => "halfelf",
                RaceType.LightfootHalfling => "lightfoothalfling",
                RaceType.StoutHalfling => "stouthalfling",
                RaceType.HalfOrc => "halforc",
                RaceType.Human => "human",
                RaceType.Tiefling => "tiefling",
                _ => "other",
            };
        }
        public static string MonsterTypeToWord(MonsterType type)
        {
            return type switch //translate MonsterType to word
            {
                MonsterType.Kobold_Orc => "kobold-orc",
                _ => "none",
            };
        }
        public static string BoolToWord(bool b)
        {
            return b ? "true" : "false"; //translate bool to word
            //return b switch //translate bool to word
            //{
                //true => "true",
                //false => "false",
            //};
        }
        public static string RoomTypeToWord(RoomType type)
        {
            return type switch //translate RoomType to word
            {
                RoomType.Normal => "normal",
                RoomType.Shop => "shop",
                RoomType.Safe => "safe",
                RoomType.Lit => "lit",
                _ => "normal",
            };
        }
        public void WriteBool(string s, bool b)
        {
            WriteWords(s, BoolToWord(b)); //write bool
        }
    }
}
