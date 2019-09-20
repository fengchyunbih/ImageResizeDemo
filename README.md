ImageResizeDemo
===

每一輪執行21張圖片，每張圖片放大2倍，每種方法各執行200次
CPU : i5-7400 3.00 GHz 4C4T


# 同步方法
類別 **ImageProcess**

原始程式

```
執行結果(對照組)
mean : 2995.66 ms
std : 87.18 ms
```

# 非同步方法1
類別 **ImageProcessAsync1**

將讀取圖片、放大圖片及儲存圖片分成個別的非同步方法個別執行

```
執行結果
mean : 2954.44 ms (-1.38%)
std : 200.44 ms
```

# 非同步方法2
類別 **ImageProcessAsync2**

將讀取圖片、放大圖片及儲存圖片整合為一個非同步方法執行

```
執行結果
mean : 2080.76 ms (-30.54%)
std : 85.09 ms
```