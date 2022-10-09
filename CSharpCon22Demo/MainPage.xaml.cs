namespace CSharpCon22Demo;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			DetectEmotionButton.Text = $"Clicked {count} time";
		else
            DetectEmotionButton.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(DetectEmotionButton.Text);
	}

	private async void CaptureImageButton_Clicked(object sender, EventArgs e)
	{
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

				CapturedImage.Source = ImageSource.FromFile(localFilePath);
            }
        }
		else
		{
			await DisplayAlert("MediaPicker", "Not Supported", "Ok");
		}
    }
}

