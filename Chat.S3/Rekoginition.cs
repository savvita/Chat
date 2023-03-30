using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Chat.S3.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Chat.S3
{
    public class Rekoginition
    {
        private readonly AmazonRekognitionClient _client;
        private readonly AmazonTranscribeServiceClient _transcribeClient;
        private readonly S3Access _access;
        public Rekoginition(string accessKey, string secretKey, Amazon.RegionEndpoint region)
        {
            _client = new AmazonRekognitionClient(accessKey, secretKey, region);
            _transcribeClient = new AmazonTranscribeServiceClient(accessKey, secretKey, region);
            _access = new S3Access(accessKey, secretKey, region);
        }

        public string RekognizeFromImage(string bucket, string objectName)
        {
            string fileInfo = "";
            DetectTextRequest request = new DetectTextRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object()
                    {
                        Name = objectName,
                        Bucket = bucket,
                    }
                }
            };

            try
            {
                DetectTextResponse detectTextResponse = _client.DetectTextAsync(request).GetAwaiter().GetResult();
                foreach (TextDetection text in detectTextResponse.TextDetections)
                {
                    if (text.Type == TextTypes.WORD)
                    {
                        if (text.DetectedText.Contains(';'))
                        {
                            text.DetectedText += "\n";
                        }
                        fileInfo += text.DetectedText + " ";
                    }
                }
            }

            catch (Exception e)
            {
                return e.Message;
            }
            return fileInfo;
        }

        public async Task<string> TranscribeMediaFile(string bucket, string objectName, string languageCode)
        {
            var pos = objectName.LastIndexOf(".");
            var transcriptFileName = (pos != -1) ? objectName.Substring(0, pos) + ".json" : objectName + ".json";

            var startJobRequest = new StartTranscriptionJobRequest()
            {
                Media = new Media()
                {
                    MediaFileUri = $"https://{bucket}.s3.eu-west-2.amazonaws.com/{objectName}"
                },
                OutputBucketName = bucket,
                OutputKey = transcriptFileName,
                TranscriptionJobName = $"{DateTime.Now.Ticks}-{objectName}",
                LanguageCode = new LanguageCode(languageCode),
            };

            var transcriptionJobResponse = await _transcribeClient.StartTranscriptionJobAsync(startJobRequest);

            if (transcriptionJobResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                return String.Empty;
            }

            var data = await _access.DownloadFromBucketAsync(bucket, transcriptFileName);

            using (var stream = new MemoryStream(data))
            {
                var res = JsonSerializer.Deserialize<TranscribeJobResult>(data);

                if(res != null && res.Results != null && res.Results.Transcripts != null && res.Results.Transcripts.Count > 0)
                {
                    return res.Results.Transcripts[0].Transcript ?? String.Empty;
                }
            }

            return String.Empty;
        }
    }
}
