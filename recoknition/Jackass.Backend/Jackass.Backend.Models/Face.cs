namespace Jackass.Backend.Models
{
    public sealed class Face
    {
        public float MatchConfidence { get; set; }
        public string Name { get; set; }
        public FaceInfo FaceInfo { get; set; }
    }
}