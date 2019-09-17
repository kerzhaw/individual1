using System.Collections.Generic;

namespace Jackass.Backend.Models
{
    public class RekognizedImage
    {
        public string Hash { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Face> Faces { get; set; }
    }
}