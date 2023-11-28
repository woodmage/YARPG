namespace YARPG
{
    public static class PathFindingService
    {
        public static List<Point> FindPath(Point start, Point end, bool isplayer)
        {
            var openset = new List<Point>(); // Create the open and closed sets
            var closedset = new HashSet<Point>();

            var gscore = new Dictionary<Point, double>(); // Create dictionaries to store the scores and parent points
            var fscore = new Dictionary<Point, double>();
            var parent = new Dictionary<Point, Point>();

            gscore[start] = 0; // Initialize scores and add the start point to the open set
            fscore[start] = HeuristicCostEstimate(start, end);
            openset.Add(start);

            while (openset.Count > 0)
            {
                var current = GetPointWithLowestFScore(openset, fscore); // Find the point with the lowest fScore in the open set

                if (current.Equals(end)) // If the current point is the goal, reconstruct the path and return it
                {
                    return ReconstructPath(parent, current);
                }

                openset.Remove(current); // Move the current point from the open set to the closed set
                closedset.Add(current);

                var neighbors = GetNeighbors(current, isplayer); // Get the neighbors of the current point

                foreach (var neighbor in neighbors)
                {

                    if (closedset.Contains(neighbor)) // If the neighbor is already in the closed set, skip it
                    {
                        continue;
                    }

                    var tentativegscore = gscore[current] + DistanceBetween(current, neighbor); // Calculate the tentative gScore for the neighbor

                    if (!openset.Contains(neighbor)) // If the neighbor is not in the open set, add it and calculate its scores
                    {
                        openset.Add(neighbor);
                        gscore[neighbor] = tentativegscore;
                        fscore[neighbor] = tentativegscore + HeuristicCostEstimate(neighbor, end);
                        parent[neighbor] = current;
                    }
                    else if (tentativegscore >= gscore[neighbor]) // If the tentative gScore is greater than the neighbor's gScore, skip it
                    {
                        continue;
                    }

                    gscore[neighbor] = tentativegscore; // Update the neighbor's scores and parent
                    fscore[neighbor] = tentativegscore + HeuristicCostEstimate(neighbor, end);
                    parent[neighbor] = current;
                }
            }

            return new(); // No path found
        }

        private static List<Point> ReconstructPath(Dictionary<Point, Point> parent, Point current)
        {
            var path = new List<Point> { current }; // Create a new list to store the path
            while (parent.ContainsKey(current)) // Iterate until the current point has a parent in the dictionary
            {
                current = parent[current]; // Update the current point to its parent
                path.Insert(0, current); // Insert the current point at the beginning of the path list
            }
            return path; // Return the reconstructed path
        }

        private static double HeuristicCostEstimate(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y); // Manhattan distance heuristic
        }

        private static double DistanceBetween(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2)); // Euclidean distance between two points
        }

        private static List<Point> GetNeighbors(Point point, bool isplayer)
        {
            var neighbors = new List<Point>();

            for (int dx = -1; dx <= 1; dx++) // Add all possible neighboring points (including diagonals)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue; // Skip the current point
                    }

                    var neighbor = new Point(point.X + dx, point.Y + dy);

                    if (Thing.CanMove(neighbor, isplayer)) // Check if the neighbor is a valid move
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        private static Point GetPointWithLowestFScore(List<Point> points, Dictionary<Point, double> fscore)
        {
            var lowestfscore = double.PositiveInfinity;
            Point? pointwithlowestfscore = null;

            foreach (var point in points)
            {
                if (fscore.ContainsKey(point) && fscore[point] < lowestfscore)
                {
                    lowestfscore = fscore[point];
                    pointwithlowestfscore = point;
                }
            }

            if (pointwithlowestfscore == null) return Point.Empty;
            return (Point)pointwithlowestfscore;
        }
    }
}
