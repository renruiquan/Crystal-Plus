# 热血传奇服务器中文配置使用说明

## 概述

本项目为热血传奇服务器添加了完整的中文界面支持，包括服务器管理界面和游戏内文本的本地化。

## 功能特性

### ✅ 已完成的功能

1. **完整的中文语言配置文件** (`Configs/Language.zh-CN.ini`)
   - 游戏内文本翻译
   - UI界面文本翻译
   - 菜单项翻译
   - 配置界面翻译

2. **UI语言管理器** (`Server.MirForms/UILanguageManager.cs`)
   - 自动加载语言文件
   - 动态应用界面翻译
   - 支持实时语言切换

3. **语言切换功能**
   - 在服务器配置界面中选择语言
   - 支持英语、中文、俄语、韩语
   - 实时应用语言更改

## 文件结构

```
Configs/
├── Language.ini          # 英文语言文件（默认）
├── Language.zh-CN.ini   # 中文语言文件
├── Language.ru-RU.ini   # 俄语语言文件
└── Language.ko-KR.ini   # 韩语语言文件

Server.MirForms/
├── UILanguageManager.cs  # UI语言管理器
├── SMain.cs             # 主界面（已集成语言支持）
└── ConfigForm.cs        # 配置界面（已集成语言支持）
```

## 使用方法

### 1. 启用中文界面

1. 启动服务器管理程序
2. 点击菜单 `Config` → `Server`
3. 在配置窗口中找到 `Language` 选项
4. 从下拉菜单中选择 `Chinese`
5. 点击 `Save` 保存设置
6. 重启服务器以应用更改

### 2. 语言切换

- **英语**: 选择 `English`
- **中文**: 选择 `Chinese`
- **俄语**: 选择 `Russian`
- **韩语**: 选择 `Korean`

### 3. 验证配置

运行测试脚本验证配置是否正确：

```powershell
powershell -ExecutionPolicy Bypass -File test_chinese_config.ps1
```

## 翻译内容

### 游戏内文本
- 欢迎消息
- 等级提示
- 装备属性
- 错误消息
- 系统通知

### UI界面文本
- 主界面标签页
- 菜单项
- 按钮文本
- 状态栏信息
- 配置界面

### 配置选项
- 网络设置
- 版本检查
- 权限控制
- 游戏平衡

## 技术实现

### 语言文件格式

```ini
[Language]
# 游戏内文本
Welcome=欢迎来到热血传奇服务器。
OnlinePlayers=在线玩家: {0}

# UI界面文本
Logs=日志
Debug Logs=调试日志
Chat Logs=聊天日志

# 菜单项
Control=控制
Start Server=启动服务器
Stop Server=停止服务器
```

### UI语言管理器

```csharp
// 加载语言文件
UILanguageManager.LoadUILanguage(Settings.LanguageFilePath);

// 应用界面翻译
UILanguageManager.ApplyUILanguage(this);
```

## 自定义翻译

### 添加新的翻译项

1. 在 `Configs/Language.zh-CN.ini` 文件中添加新的键值对：
   ```ini
   NewTextKey=新的中文文本
   ```

2. 在代码中使用：
   ```csharp
   string translatedText = UILanguageManager.GetText("NewTextKey", "默认文本");
   ```

### 修改现有翻译

直接编辑 `Configs/Language.zh-CN.ini` 文件中对应的值即可。

## 故障排除

### 常见问题

1. **界面没有显示中文**
   - 检查语言文件是否存在
   - 确认在配置中选择了中文
   - 重启服务器应用更改

2. **部分文本没有翻译**
   - 检查语言文件中是否包含对应的键
   - 确认键名与界面文本完全匹配

3. **编码问题**
   - 确保语言文件使用UTF-8编码
   - 检查文件是否包含BOM标记

### 调试方法

1. 运行测试脚本检查文件完整性
2. 查看服务器日志中的语言加载信息
3. 使用调试模式检查语言管理器状态

## 更新日志

### v1.0.0 (当前版本)
- ✅ 完整的中文界面支持
- ✅ 多语言切换功能
- ✅ UI语言管理器
- ✅ 配置界面本地化
- ✅ 测试脚本和文档

## 贡献

欢迎提交翻译改进和功能建议！

## 许可证

本项目遵循原项目的许可证条款。 