using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongMingDe
{
    internal static class Helper
    {
        public static string ToBinary(this byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                builder.Append(Convert.ToString(data[i], 2) + " ");
            }
            return builder.ToString().Trim();
        }
        
        public static readonly string HELP_TEXT = $@"指令帮助：
名称        英文        别名        介绍
版本        version    (无)        查看当前软件版本
                                            和当前系统版本。
说           say          echo       输出一串字符串。
退出       exit          quit        退出当前程序。
返回上一级                           回到上一级文件
回到上一级                           夹。
退回上一级   cd ..   (无)
到           cd           (无)         将当前工作路径设
                                            为另一个路径，或
                                            者进入到当前文件
                                            夹下的一个文件夹
                                            中。
清屏        clear       cls           清除当前输出的内
                                             容。
扫描        dir          (无)          查看当前文件夹下
                                             的所有文件夹和文
                                             件。
启动        start       (无)          启动一个可执行文
                                            件或用默认方式打
                                            开一个文件。
设置        set         (无)          设置当前程序中的
                                             一些变量。
重置        reset      (无)          重置当前程序中的
                                            所有可设置的变量。
当前文件夹   cf      cp           获取当前工作文件
当前路径                              夹的信息。
删除        delete    del          删除一个文件夹或
                                            一个文件。
查看        read       rd           查看一个文件中的
                                            文本。
查看二进制 readbin rbin      以二进制的方式查
                                            看一个文件。
新建文件 createfile crf         新建一个文件。
创建文件
新建文件夹 createdirectory 新建一个文件夹。
创建文件夹            crd
写入文件 writefile  wrf         在当前软件中编
                                            辑文件内容。
执行       execute    (无)        执行一个适用于
                                            “聪明的”的批处
                                            理文件。
颜色       color         (无)        设置终端的前景
                                             色和背景色。
帮助 help ?(半角)或？(全角)  查看可用帮助。
<文件名>  (无)       (无)         执行在环境变量“
                                             PATH”中的某个
                                             值下的指定可执
                                             行文件，或者是
                                             当前程序所在目
                                             录的指定可执行
                                             文件。
-----------------------------------------------
快捷键帮助：
键              介绍
Ctrl+Q      完全清除屏幕，直接在最开始的位
                 置输入指令。
Ctrl+E       如果在文件编辑页面，则保存并返
                 回终端；否则退出终端。
Ctrl+T       快速地插入四个空格。
Ctrl+R       重置当前程序中的所有可设置的变
                 量，效果与“重置”指令相当。";
    }
}
