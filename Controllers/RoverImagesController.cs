using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace MarsRoverImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoverImagesController : ControllerBase
    {
        string baseUrl = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?earth_date=%date%&api_key=DEMO_KEY";

        [Route("getimagesfordate")]
        public async Task<List<byte[]>> GetImagesForDate(string dateParam)
        {
            
            var result = new List<byte[]>(); 
            var date = Convert.ToDateTime(dateParam).ToString("yyyy-MM-dd");
            baseUrl = baseUrl.Replace("%date%", date);
            
            using (var client = new HttpClient())
            {
                var resultList = new List<string>();
                var clientResponse = await client.GetAsync(baseUrl);
                var content = await clientResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(content);

                foreach (var image in responseObject.photos)
                {
                    //get image bytes
                    string image_url = image.img_src.ToString();
                    resultList.Add(image_url);
                    var webClient = new WebClient();
                    var imageBytes = webClient.DownloadData(image_url);

                    result.Add(imageBytes);

                    //save file to disk
                    var nameParsed = image_url.Split("/");
                    var fileName = nameParsed[nameParsed.Length - 1];
                    if (!System.IO.File.Exists(fileName))
                    {
                        System.IO.File.WriteAllBytes("ImageFiles/" + fileName, imageBytes);
                    }
                } 

                return result;
            }

        }

        [Route("getimageurls")]
        public async Task<List<string>> GetImageUrls(string dateParam)
        {
            var date = Convert.ToDateTime(dateParam).ToString("yyyy-MM-dd");
            baseUrl = baseUrl.Replace("%date%", date);
            
            using (var client = new HttpClient())
            {
                var resultList = new List<string>();
                var clientResponse = await client.GetAsync(baseUrl);
                var content = await clientResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(content);

                foreach (var image in responseObject.photos)
                {
                    string image_url = image.img_src.ToString();
                    resultList.Add(image_url);
                }

                return resultList;
            }

        }

        [Route("getimagesfordatafile")]
        public async Task<List<byte[]>> GetImagesForDataFile()
        {
            var line = "";
            var result = new List<byte[]>(); ;
            var dataFile = new StreamReader(@"DataFile\dates.txt");
            while ((line = dataFile.ReadLine()) != null)
            {
                try
                {
                    var dateImage = await GetImagesForDate(line);
                    foreach (var image in dateImage)
                    {
                        result.Add(image);
                    }
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            dataFile.Close();
            return result;
        }
    }
}