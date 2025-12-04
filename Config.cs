using System.Text.Json.Serialization;

namespace GCodeProcessor
{
    public class Config
    {
        [JsonPropertyName("offsetX")]
        public double OffsetX { get; set; } = 55.0;

        [JsonPropertyName("offsetY")]
        public double OffsetY { get; set; } = 55.0;

        [JsonPropertyName("countX")]
        public int CountX { get; set; } = 1;

        [JsonPropertyName("countY")]
        public int CountY { get; set; } = 1;

        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "x";

        [JsonPropertyName("lastInputFile")]
        public string LastInputFile { get; set; } = "";

        [JsonPropertyName("outputFormat")]
        public string OutputFormat { get; set; } = "cnc";
    }
}
