import os
import re
import argparse

# 生成完之后使用DeepSeek生成翻译
# 提示词:请根据附件，Language.zh-CN.ini文件，首行为[Language]，其他行key=原字符串，Value=翻译后的中文，例如    Chinese = 中文

def find_and_save_text_assignments(root_dir, output_file):
    # 正则表达式模式：匹配 .Text = 后面跟着非分号的内容直到分号前
    pattern = re.compile(r'\.Text\s*=\s*([^;]+);')
    
    with open(output_file, 'w', encoding='utf-8') as out_file:
        # 遍历根目录及其子目录
        for root, _, files in os.walk(root_dir):
            for file in files:
                if file.endswith('.Designer.cs'):
                    file_path = os.path.join(root, file)
                    try:
                        with open(file_path, 'r', encoding='utf-8') as f:
                            content = f.read()
                            # 查找所有匹配项
                            matches = pattern.findall(content)
                            if matches:
                                out_file.write(f"// 文件: {file_path}\n")
                                for match in matches:
                                    out_file.write(f"{match.strip()}\n")
                                out_file.write("\n")  # 添加空行分隔不同文件的结果
                    except Exception as e:
                        print(f"处理文件 {file_path} 时出错: {e}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='查找.Designer.cs文件中的.Text赋值语句')
    parser.add_argument('root_dir', help='要搜索的根目录')
    parser.add_argument('output_file', help='输出文本文件路径')
    
    args = parser.parse_args()
    
    find_and_save_text_assignments(args.root_dir, args.output_file)
    print(f"处理完成，结果已保存到 {args.output_file}")