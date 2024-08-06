# API 使用說明文件

## API 概述
本 API 用於上傳與刪除資料。

### URL
Post https://www.sol-idea.com.tw/back/api/JsonUploadInputApi

### 請求格式
| KEY            | VALUE                |
| -------------- | -------------------- |
| Content-Type   | multipart/form-data  |

### 請求資料範例
#### Layer 1
| KEY            | VALUE                |
| -------------- | -------------------- |
| uploadedJsonVM | json 字串化後的字典，範例 |
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
| FolderSn              | 要新增或刪除的檔案所在的資料集編號，可以在網址列看到，如圖: ![image](https://github.com/user-attachments/assets/8a5d0c75-d913-44f4-a326-7ecf30c81659)|
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
curl https://www.sol-idea.com.tw/back/api/CompletionBot/SimplifiedFAQ --form jsonChatRoomVM="{\"ApiKey\":\"your_key\", \"ResponseFormat\":0, \"LogChatLogHistorySN\":-1,\"ChatLogs\":[{\"HumanContent\": \"你好阿\", }]}"
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
| FileName             | 進系統之後的檔案名稱，建議存起來以後要刪掉的時候可以用           |
| FolderSn             | 進系統之後的資料集編號，建議存起來以後要刪掉的時候可以用      |

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
| 3002                 | 資料格式錯誤 (UploadedJsonDatas FieldTitle FieldTime FieldContent CusField都不能有空值)                |
| 3005                 | 檔案過大(目前是10 MB)                 |
| 4001                 | ApiKey錯誤或不存在               |
| 4005                 | 帳號空間不足                 |
| 4006                 | 原始資料集編號沒有權限或不存在                  |
