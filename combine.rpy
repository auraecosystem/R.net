import os
import re
from bs4 import BeautifulSoup

def parse_hhc(hhc_path):
    """Extracts ordered HTML file paths from the CHM Table of Contents (.hhc)."""
    if not os.path.exists(hhc_path):
        print(f"No .hhc file found at {hhc_path}. Falling back to alphabetical order.")
        return [f for f in os.listdir('.') if f.endswith(('.html', '.htm'))]
    
    with open(hhc_path, 'r', encoding='utf-8', errors='ignore') as f:
        soup = BeautifulSoup(f.read(), 'html.parser')

    ordered_files = []
    for param in soup.find_all('param', attrs={'name': 'Local'}):
        file_path = param.get('value')
        if file_path and file_path not in ordered_files:
            # Clean relative path separators
            cleaned_path = file_path.replace('\\', '/')
            if os.path.exists(cleaned_path):
                ordered_files.append(cleaned_path)
    return ordered_files

def combine_html_files(file_list, output_file='combined.html'):
    """Combines multiple HTML files into a single document."""
    combined_body = []
    
    for file_path in file_list:
        try:
            with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                soup = BeautifulSoup(f.read(), 'html.parser')
                body = soup.find('body')
                if body:
                    # Inject a section anchor derived from file name
                    section_id = os.path.splitext(os.path.basename(file_path))[0]
                    combined_body.append(f'<section id="{section_id}">\n{body.decode_contents()}\n</section>')
        except Exception as e:
            print(f"Skipping {file_path}: {e}")

    full_html = f"""<!DOCTYPE html>
<html>
<head><meta charset="utf-8"><title>Combined Output</title></head>
<body>
{"\n<hr/>\n".join(combined_body)}
</body>
</html>"""

    with open(output_file, 'w', encoding='utf-8') as f:
        f.write(full_html)
    print(f"Combined {len(ordered_files)} files into {output_file}")

# Execution
hhc_files = [f for f in os.listdir('.') if f.endswith('.hhc')]
hhc_path = hhc_files[0] if hhc_files else 'Table of Contents.hhc'

ordered_files = parse_hhc(hhc_path)
combine_html_files(ordered_files)
