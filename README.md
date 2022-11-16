# WpfControlSamples

.NET Core 7 + WPF Control Samples

Created in 2020/05

Updated in 2022/11


## Websites

[Microsoft Docs WPF](https://docs.microsoft.com/ja-jp/dotnet/framework/wpf/)

[かずきのBlog@hatena / WPF4.5入門 その62「まとめ」](https://blog.okazuki.jp/entry/2014/12/27/200015)

[Gushwell's Dev Notes / WPFサンプル・目次](http://gushwell.ldblog.jp/archives/52313900.html)

[Ararami Studio / C#開発技術](https://araramistudio.jimdo.com/%E6%8A%80%E8%A1%93%E6%83%85%E5%A0%B1/c-%E9%96%8B%E7%99%BA%E6%8A%80%E8%A1%93/)

[ItemsControl 攻略 ～ 外観のカスタマイズ](http://grabacr.net/archives/1240)

[創造的プログラミングと粘土細工](http://pro.art55.jp/)

[【WPF】Pathで線形を書く（直線のみ）](https://qiita.com/LemonLeaf/items/c089bbb497f53b59e0b4)

[C#+WPFチューニング戦記](http://proprogrammer.hatenadiary.jp/)

[Msを16倍出し抜くwpf開発1回目](https://www.slideshare.net/cct-inc/ms16wpf1)

[MVVMパターンの常識 ― 「M」「V」「VM」の役割とは？](https://www.atmarkit.co.jp/fdotnet/chushin/greatblogentry_02/greatblogentry_02_01.html)

[WPF でフォルダー選択のダイアログを選択・実装する](https://shikaku-sh.hatenablog.com/entry/wpf-folder-selection-dialog)

[【WPF】ViewModelがINotifyPropertyChangedを実装していないとメモリリークする件 - aridai.NET](https://aridai.net/articles/15.html)

### Per-Monitor DPI

[Developing a Per-Monitor DPI-Aware WPF Application - MSDN](http://msdn.microsoft.com/en-us/library/windows/desktop/ee308410(v=vs.85).aspx)

[デスクトップアプリの高DPI対応 #1 – 高DPI対応とは](https://nishy-software.com/ja/dev-sw/windows-high-dpi-desktop-app-1/)

[デスクトップアプリの高DPI対応 #3 – WPFアプリ](https://nishy-software.com/ja/dev-sw/windows-high-dpi-desktop-app-3/)

[タスク マネージャーでアプリのDPI対応を確認可能 - 窓の杜](https://forest.watch.impress.co.jp/docs/news/1148530.html)

[Per-Monitor DPI 環境下で WPF の Window 位置を調整する - しばやん雑記](https://blog.shibayan.jp/entry/20190612/1560267647)

[Windows 8.1 で加わった Per-Monitor DPI と WPF での対応方法 - grabacr.nét](http://grabacr.net/archives/1132)

[.Net 4.6.2以降でのWPFのPer-Monitor DPI対応 - SourceChord](http://sourcechord.hatenablog.com/entry/2016/12/14/081112)


### stackoverflow

[What's the difference betwen a UserControl and a ContentControl?](https://stackoverflow.com/questions/18781679/whats-the-difference-betwen-a-usercontrol-and-a-contentcontrol)

### Assets

[Ionicons](https://ionicons.com/)

[MATERIAL DESIGN](https://material.io/resources/icons/?style=baseline)

[Transforming SVG graphics to XAML Metro Icons](https://blogs.u2u.be/diederik/post/Transforming-SVG-graphics-to-XAML-Metro-Icons)

[MahApps/MahApps.Metro.IconPacks: Awesome icon packs for WPF and UWP in one library](https://github.com/MahApps/MahApps.Metro.IconPacks)

### GitHub

[Carlos487/awesome-wpf](https://github.com/Carlos487/awesome-wpf)

[Microsoft/XamlBehaviorsWpf](https://github.com/Microsoft/XamlBehaviorsWpf)

[aybe/Windows-API-Code-Pack-1.1](https://github.com/aybe/Windows-API-Code-Pack-1.1)

[snoopwpf/snoopwpf: Snoop - The WPF Spy Utility](https://github.com/snoopwpf/snoopwpf)

Ctrl + Shift で詮索対象のコントロールに移動します。

## Memo

**[.NET5]自己完結リリースで単一ファイルにならない対応**

.NET5 で `PublishSingleFile` を `True` で Publish したときに、9個くらい dll (*1) ができちゃう対策は、`IncludeNativeLibrariesForSelfExtract` を `True` にする。

*1) clrcompression.dll clrjit.dll coreclr.dll D3DCompiler_47_cor3.dll mscordaccore.dll PenImc_cor3.dll PresentationNative_cor3.dll vcruntime140_cor3.dll wpfgfx_cor3.dll

参考：[.NET 5でシングルバイナリを作る | OPCDiary](https://opcdiary.net/net-5%E3%81%A7%E3%82%B7%E3%83%B3%E3%82%B0%E3%83%AB%E3%83%90%E3%82%A4%E3%83%8A%E3%83%AA%E3%82%92%E4%BD%9C%E3%82%8B/)

PATH : `Properties\PublishProfiles\FolderProfile.pubxml`

```xml
<?xml version="1.0" encoding="utf-8"?>
<!--
https://go.microsoft.com/fwlink/?LinkID=208121. 
-->
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <Configuration>Release</Configuration>
    <Platform>Any CPU</Platform>
    <PublishDir>bin\Release\net6.0-windows\publish\</PublishDir>
    <PublishProtocol>FileSystem</PublishProtocol>
    <TargetFramework>net6.0-windows</TargetFramework>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <SelfContained>false</SelfContained>
    <PublishSingleFile>True</PublishSingleFile>
    <PublishReadyToRun>False</PublishReadyToRun>
    <PublishTrimmed>False</PublishTrimmed>
    <IncludeNativeLibrariesForSelfExtract>True</IncludeNativeLibrariesForSelfExtract>
  </PropertyGroup>
</Project>
```

**発行の出力言語指定（動作未確認）**

[Microsoft.NET.Sdk の MSBuild プロパティ - .NET | Microsoft Docs](https://docs.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#satelliteresourcelanguages)

```xml
<PropertyGroup>
  <SatelliteResourceLanguages>ja-JP;en-US</SatelliteResourceLanguages>
</PropertyGroup>
```

**ビルド時にリソースをコピーする**

`*.csproj` の記述。プロジェクトのプロパティから設定しても良いです。

```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
  <Exec Command="xcopy /D /E /C /S /Y $(SolutionDir)\assets\* $(OutDir)" />
</Target>
```

```
/D コピー元の日付がコピー先の日付より新しいファイルだけをコピーします。
/E ディレクトリまたはサブディレクトリが空であってもコピーします。
/C エラーが発生してもコピーを続けます。
/S 空の場合を除いて、ディレクトリとサブディレクトリをコピーします。
/Y 既存のファイルを上書きする前に確認のメッセージを表示しません。
```

EOF

