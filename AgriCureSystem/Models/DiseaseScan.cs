namespace AgriCureSystem.Models
{
    public class DiseaseScan
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string ScanImg { get; set; }
        public string Prediction { get; set; }
        public string Confidence { get; set; }
        public string Description { get; set; }
        public string Symptoms { get; set; }
        public string Treatment { get; set; }
        public DateTime ScanDate { get; set; } = DateTime.Now;
    }
}
