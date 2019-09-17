using System.Collections.Generic;

namespace Jackass.Backend.Models
{
    public sealed class FaceInfo
    {
        public List<Landmark> Landmarks { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public float Confidence { get; set; }
        public Pose Pose { get; set; }
        public ImageQuality ImageQuality { get; set; }
    }
}