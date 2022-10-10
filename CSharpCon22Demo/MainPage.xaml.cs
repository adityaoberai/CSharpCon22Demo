using CSharpCon22Demo.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace CSharpCon22Demo;

public partial class MainPage : ContentPage
{
    public string filePath;

    public MainPage()
	{
		InitializeComponent();
	}

	private async void CaptureImageButton_Clicked(object sender, EventArgs e)
	{
        if (MediaPicker.Default.IsCaptureSupported)
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

				CapturedImage.Source = ImageSource.FromFile(localFilePath);
                filePath = localFilePath;
            }
        }
		else
		{
			await DisplayAlert("MediaPicker", "Not Supported", "Ok");
		}
    }

    private async void ExtractTextButton_Clicked(object sender, EventArgs e)
    {
        var credentials = new CognitiveServicesHelper();
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(credentials.ComputerVisionAPIKey)) { Endpoint = credentials.ComputerVisionAPIEndpoint };

        // Read text using local image path
        using FileStream capture = File.OpenRead(filePath);
        var textHeaders = await client.ReadInStreamAsync(capture);

        // After the request, get the operation location (operation ID)
        string operationLocation = textHeaders.OperationLocation;
        Thread.Sleep(2000);

        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

        // Extract the text
        ReadOperationResult results;
        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

        // Display the found text.
        var textUrlFileResults = results.AnalyzeResult.ReadResults;
        string output = "";
        foreach (ReadResult page in textUrlFileResults)
        {
            foreach (Line line in page.Lines)
            {
                output += line.Text + "\n";
            }
        }
        await DisplayAlert("Output", output, "Ok");
    }
}

