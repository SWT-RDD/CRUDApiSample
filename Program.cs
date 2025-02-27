using Newtonsoft.Json;

//生成範例兩筆資料
var data1 = new UploadedJsonData()
{
    FieldTitle = "標題1",
    FieldTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    FieldContent = "我是內容1",
    CusField = ["1", "2", "3"]
};
var data2 = new UploadedJsonData()
{
    FieldTitle = "標題2",
    FieldTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    FieldContent = "我是內容2",
    CusField = ["1", "2", "3"]
};



//主程式(新增)
//----------------------------------------------------------
UploadedJsonVM uploadedJsonVM = new UploadedJsonVM()
{
    FileName = "檔案名稱",
    ApiKey = "your_key",
    FolderSn = 1234,
    Action = (int)UploadedJsonAction.Create,
    UploadedJsonDatas = [data1, data2]
};
await PostUploadedJsonVM(uploadedJsonVM);
//----------------------------------------------------------



//主程式(刪除)
//----------------------------------------------------------
//UploadedJsonVM uploadedJsonVM = new UploadedJsonVM()
//{
//    FileName = "20240731154038_檔案名稱.jsonstring",
//    ApiKey = "your_key",
//    FolderSn = 1234,
//    Action = (int)UploadedJsonAction.Delete,
//    UploadedJsonDatas = [] //不能漏掉這項
//};
//await PostUploadedJsonVM(uploadedJsonVM);
//----------------------------------------------------------




//HttpPost副程式
async Task PostUploadedJsonVM(UploadedJsonVM uploadedJsonVM)
{
    var url = "https://gufofaq.gufolab.com/api/JsonUploadInputApi";
    var jsonInputFile = JsonConvert.SerializeObject(uploadedJsonVM);
    MultipartFormDataContent form = new MultipartFormDataContent();
    form.Add(new StringContent(jsonInputFile), "jsonInputFile");

    using (var client = new HttpClient())
    {
        var response = await client.PostAsync(url, form);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonFormat = JsonConvert.DeserializeObject<JsonFormat>(responseContent);
            var returnJsonData = JsonConvert.DeserializeObject<ReturnJsonData>(jsonFormat.JsonData);
            Console.WriteLine(returnJsonData.FolderSn + " " + returnJsonData.FileName);
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ECustomError>(errorContent);
                Console.WriteLine($"Handled Error: {error.Message}, Code: {error.Code}");
            }
            else
            {
                Console.WriteLine($"Failed to post chatRoomVM. Status code: {response.StatusCode}");
            }
        }
    }
}



//資料結構
public enum UploadedJsonAction : int
{
    Create = 0,
    Delete = 2
}

public class JsonFormat
{
    public string JsonData { get; set; }
}
public class UploadedJsonData
{
    public string FieldTitle { get; set; }
    public string FieldTime { get; set; }
    public string FieldContent { get; set; }
    public string[] CusField { get; set; }
}
public class UploadedJsonVM
{
    public string ApiKey { get; set; }
    public string FileName { get; set; }
    public int FolderSn { get; set; }
    public int Action { get; set; }
    public UploadedJsonData[] UploadedJsonDatas { get; set; }
}
public class ReturnJsonData
{
    public string FileName { get; set; }
    public int FolderSn { get; set; }
}
public class ECustomError
{
    public string Message { get; set; }
    public string Code { get; set; }
}
