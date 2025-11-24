# API 使用說明文件

## API 概述
本 API 提供資料上傳、刪除以及文字萃取（國家/時間）功能。

---

## 1. 資料上傳與刪除 API

### URL
Post https://gufofaq.gufolab.com/api/JsonUploadInputApi

### 請求格式
| KEY            | VALUE                |
| -------------- | -------------------- |
| Content-Type   | multipart/form-data  |

### 請求資料範例
#### Layer 1
| KEY            | VALUE                |
| -------------- | -------------------- |
| jsonInputFile  | json 字串化後的字典，範例 |
|                | {                   |
|                | "ApiKey": "your_key",|
|                | "FileName": "檔案名稱",|
|                | "FolderSn": 1234,|
|                | "Action": 0,|
|                | "UploadedJsonDatas": [{"FieldTitle": "標題1", "FieldTime": "2024-08-06 11:11:11", "FieldContent": "我是內容1", "CusField": ["備註1", "備註2", "備註3"]}]|
|                | }                   |

#### Layer 2
| KEY                   | VALUE                       |
| --------------------- | --------------------------- |
| ApiKey                | 你的 api key                |
| FileName              | 要新增或刪除的檔案名稱            |
| FolderSn              | 要新增或刪除的檔案所在的原始資料集編號，可以在網址列看到，如圖: ![image](https://github.com/user-attachments/assets/8a5d0c75-d913-44f4-a326-7ecf30c81659)|
| Action                | 選擇要新增還是刪除，新增填0，刪除填2     |
| UploadedJsonDatas     | 要上傳的資料，如果是刪除填空陣列。一次建議1000筆資料以內，不然會要等很久     |

#### Layer 3
| KEY                   | VALUE                       |
| --------------------- | --------------------------- |
| FieldTitle            | 標題欄位，如果想要空值可以填空字串             |
| FieldTime             | 時間欄位，格式yyyy-MM-dd HH:mm:ss，如果想要空值可以填空字串            |
| FieldContent          | 內容欄位，非常不建議空值|
| CusField              | 備註欄位，列表中需要填三個字串，如果任一個備註想要空值可以填空字串，反正最後列表中要有三個字串     |

### curl 請求範例
```
curl https://gufofaq.gufolab.com/api/JsonUploadInputApi --form jsonInputFile="{\"ApiKey\":\"your_key\", \"FileName\":\"檔案名稱\", \"FolderSn\":1234,\"Action\":0,\"UploadedJsonDatas\":[{\"FieldTitle\": \"標題1\",\"FieldTime\": \"2024-08-06 11:11:11\",\"FieldContent\": \"我是內容1\",\"CusField\":[\"備註1\",\"備註2\",\"備註3\"], }]}"
```
記得換掉your_key
### 回應資料範例
#### Layer 1
| KEY        | VALUE                      |
| ---------- | -------------------------- |
| JsonData   | json 字串化後的資料，範例     |
|            | {                         |
|            | "FileName": "20240806170155_檔案名稱.jsonstring",      |
|            | "FolderSn": 1234|
|            | }                         |

#### Layer 2
| KEY                  | VALUE                     |
| -------------------- | ------------------------- |
| FileName             | 資料進到系統之後的檔案名稱，建議存起來以後要刪掉的時候可以用           |
| FolderSn             | 資料進到系統之後的原始資料集編號，建議存起來以後要刪掉的時候可以用      |

### 回應錯誤處理
使用 Http 400 Bad Request

#### 錯誤格式
| KEY                  | VALUE                     |
| -------------------- | ------------------------- |
| Code                 | 錯誤代碼                  |
| Message              | 錯誤訊息                  |

#### 錯誤列表
| Code                 | Message                   |
| -------------------- | ------------------------- |
| 3001                 | Json字串解析失敗                |
| 3002                 | 資料格式錯誤 (UploadedJsonDatas內容不能為空 FieldTitle FieldTime FieldContent CusField都不能有空值null)                |
| 3005                 | 檔案過大 (目前是10 MB)                 |
| 4001                 | ApiKey錯誤、不存在或過期               |
| 4005                 | 帳號空間不足                 |
| 4006                 | 原始資料集編號沒有權限或不存在                  |

---

## 2. 文字萃取 API (國家/時間)

### URL
Post https://gufofaq.gufolab.com/api/JsonUploadInputApi/ExtractCountryOrTime

### API 功能
從文字內容中萃取國家和/或時間資訊。

### 請求格式
| KEY            | VALUE                |
| -------------- | -------------------- |
| Content-Type   | multipart/form-data  |

### 請求參數
| KEY            | 必填  | VALUE                |
| -------------- | ---- | -------------------- |
| Text           | 是   | 要分析的文字內容（2-5000 字元）|
| Mode           | 是   | 萃取模式："1"、"2" 或 "3" |
| ApiKey         | 是   | 你的 API Key          |
| TimeReference  | 否   | 發文時間參考（可選）      |

### Mode 說明
| Mode | 功能說明           | 回傳內容              |
| ---- | ---------------- | ------------------- |
| 1    | 只萃取國家         | Country（國家或"無"）<br>Time 為 null |
| 2    | 只萃取時間         | Time（時間或"無"）<br>Country 為 null |
| 3    | 同時萃取國家和時間  | Country（國家或"無"）<br>Time（時間或"無"）|

### curl 請求範例
```bash
curl https://gufofaq.gufolab.com/api/JsonUploadInputApi/ExtractCountryOrTime \
  --form Text="2024年3月15日，美國總統在白宮發表演說" \
  --form Mode="3" \
  --form ApiKey="your_api_key" \
  --form TimeReference="2024-03-15 10:00:00"
```
記得換掉 your_api_key

### 回應資料範例
#### 成功回應格式
```json
{
  "Error": false,
  "Message": "萃取成功",
  "JsonData": "{\"Country\":\"美國\",\"Time\":\"2024-03-15 00:00:00\"}"
}
```

#### JsonData 內容說明
| KEY      | VALUE                          |
| -------- | ------------------------------ |
| Country  | 萃取到的國家名稱，或"無"（若 Mode 為 2 則為 null）|
| Time     | 萃取到的時間（格式：yyyy-MM-dd 或 yyyy-MM-dd HH:mm:ss），或"無"（若 Mode 為 1 則為 null）|

### 回傳範例說明
1. **Mode = 1（只萃取國家）**
   ```json
   {
     "Error": false,
     "Message": "萃取成功",
     "JsonData": "{\"Country\":\"美國\",\"Time\":null}"
   }
   ```

2. **Mode = 2（只萃取時間）**
   ```json
   {
     "Error": false,
     "Message": "萃取成功",
     "JsonData": "{\"Country\":null,\"Time\":\"2024-03-15 00:00:00\"}"
   }
   ```

3. **Mode = 3（同時萃取）**
   ```json
   {
     "Error": false,
     "Message": "萃取成功",
     "JsonData": "{\"Country\":\"美國\",\"Time\":\"2024-03-15 00:00:00\"}"
   }
   ```

4. **沒有找到資訊時**
   ```json
   {
     "Error": false,
     "Message": "萃取成功",
     "JsonData": "{\"Country\":\"無\",\"Time\":\"無\"}"
   }
   ```

### 回應錯誤處理
使用 Http 400 Bad Request 或 500 Internal Server Error

#### 錯誤格式
| KEY      | VALUE      |
| -------- | ---------- |
| Code     | 錯誤代碼    |
| Message  | 錯誤訊息    |

#### 錯誤列表
| Code | Message                        |
| ---- | ------------------------------ |
| 3001 | 請求資料無效                     |
| 3007 | 文字長度無效（需要 2-5000 字元）   |
| 4001 | ApiKey 錯誤、不存在或過期         |
| 4007 | 萃取次數已超過上限                |
| 5000 | 伺服器內部錯誤                   |

### 注意事項
1. Text 長度必須在 2-5000 字元之間
2. Mode 必須是 "1"、"2" 或 "3"（字串格式）
3. TimeReference 為可選參數，提供發文時間可以幫助更準確地判斷相對時間
4. 回傳的時間格式會經過驗證，只接受 yyyy-MM-dd 或 yyyy-MM-dd HH:mm:ss 格式
5. 若萃取不到資訊，會回傳"無"而不是 null（在該 Mode 需要回傳的字段中）
6. 每次呼叫會消耗一次萃取額度，請注意配額限制
