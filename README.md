#使用群組性或電話號碼條件發送GSM#

# 介紹 #

### GSM是甚麼? ###
全球行動通訊系統（Global System for Mobile Communications)

### GSM優勢 ###
**GSM**優勢在於提供更高的數位語音品質和替代呼叫的低成本的新選擇（比如簡訊）。<br>從網路業者角度看來，其優勢是能夠部署來自不同廠商的裝置，因為GSM作為開放標準提供了更容易的互操作性。

### GSM又稱身分識別模組 ###
身分識別模組(SIM)是一個儲存使用者資料和電話本的可拆卸智慧卡IC。<br>使用者可更換手機後還能儲存自己的資訊。換句話說使用者也可以使用現在的手機而使用不同業者的SIM卡。

### GSM安全 ###
GSM被設計具有中等安全水平。系統設計使用共享金鑰使用者認證。使用者與基站之間的通訊可以被加密。演進的UMTS引入可選的USIM－使用更長鑑別金鑰保證更好的安全以及網路和使用者的雙向驗證。GSM只有網路對使用者的驗證（而不是雙向驗證）。雖然安全模組提供了保密和鑑別功能，但是鑑別能力有限而且可以偽造。

# 硬體設備 #

- 3G Modem x 1
- Sim Card x 1

# Server #

Server執行時,系統會一直等待**Client**給予**Server**訊號,此訊號傳入Server時會分類為三類

1. 群組:<br>
Server會利用此方式,檢測Client傳入的群組是否與Access內部資料為真，<br>
如為真Server會依照順序發送名單，如為否系統會自動刪除此訊息。

2. 電話:<br>
Server會利用此方式,檢測Client傳入的電話號碼是否與Access內部資料為真，<br>
如為真Server會發送名單，如為否系統會自動刪除此訊息。

3. IP:<br>
Server接收到Client訊號時，如IP非Access中就無法進入**群組**或**電話**類別。

## Server執行步驟

1. 文字:

2. 圖片:

# Client #

Client執行方式為利用命令提示(CMD) 做為傳送並且帶參數，參數種類有四種

1. Phone

2. Phone,Group

3. Group,Group

4. Group

除以上之外可依照您的方式搭配參數,以上四種為基本參數，請務必記得每個參數與參數之間需要用逗點分開。

## Client執行步驟

1. 圖片:

# Registry #

本專案使用Registry原因是利用此方式每次Server會自動抓取Comport。

# 資料參考 #
[https://zh.wikipedia.org/wiki/GSM](https://zh.wikipedia.org/wiki/GSM "GSM( WIKI )")

# 製作者 #

NianBaoZou	( nexstar@gmail.com )<br>
3/17/2016 3:29:41 PM 