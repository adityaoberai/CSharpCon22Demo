## Create Cognitive Services Helper

* Create a file `CognitiveServicesHelper.cs`

* Add the following code:

```csharp
namespace CSharpCon22Demo.Helpers
{
    public class CognitiveServicesHelper
    {
        public string ComputerVisionAPIKey { get; set; } = "<ADD COMPUTER VISION API KEY>";
        public string ComputerVisionAPIEndpoint { get; set; } = "<ADD COMPUTER VISION API ENDPOINT>";
    }
}
```

* Enter the Computer Vision API Key and Endpoint you get from your Azure Portal