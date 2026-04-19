namespace AgriCureSystem.ViewModels
{
    public class AiPredictionResponse
    {
        public string plant { get; set; }
        public string prediction { get; set; }
        public string confidence { get; set; }
        public AiPredictionDetails details { get; set; }
    }
}
