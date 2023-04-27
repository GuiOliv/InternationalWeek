using Microsoft.AspNetCore.SignalR;
using static WebAPP.MLModel;
using System.Speech.Synthesis;

namespace WebAPP.Hubs
{
    public class PredictionHub : Hub
    {
        public async Task<string> SendImage(byte[] imageData)
        {
            try
            {
                ModelInput input = new ModelInput();
                input.ImageSource = imageData;
                var result = Predict(input);
                var resultObject = result.Score.Max() >= 0.50 ? result.PredictedLabel + " - " + result.Score.Max() * 100 + "%" : "";
                new SpeechSynthesizer().Speak(result.PredictedLabel);
                await Clients.All.SendAsync("ReceiveMessage", resultObject);
                return resultObject;
            }
            catch (Exception ex)
            {
                // Log the error or return a custom error message to the client
                return "Error processing image: " + ex.InnerException + " and " + ex.InnerException.InnerException.Message;
            }
        }
    }
}
