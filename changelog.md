# 更新日志

## `v0.1.4`

>更新日期：2024-01-19
>
>主要修复 bug : issue@1

- [x] 代码重新格式化，
- [x] 部分变量重命名
- [x] 修复Unix 下 `UnicastIPAddressInformation.DuplicateAddressDetectionState` 获取时提示不支持的问题，针对目前未处理的识别异常将忽略网卡是否有效的判断
- [x] 获取本地网卡时，增加跳过未启用的网卡的判断
- [x] 增加 IPV6 地址的返回

