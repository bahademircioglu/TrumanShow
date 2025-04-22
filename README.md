# TrumanShow

C# Console Application for Text‑Based Q&A

## Overview

Reads a script and a set of questions, then finds and prints answers by searching the text.

## Features

- Loads plain text script (`the_truman_show_script.txt`)  
- Parses and answers questions from `questions.txt`  
- Outputs answers with question numbering  

## Prerequisites

- .NET 6.0 SDK or higher
- Windows, macOS, or Linux

## Installation

```bash
git clone https://github.com/bahademircioglu/TrumanShow.git
cd TrumanShow
dotnet build
```

## Usage

Place the script and questions files alongside the executable:

```bash
cd bin/Debug/net6.0/
./TrumanShow.exe ../the_truman_show_script.txt ../questions.txt
```

## Project Structure

```
TrumanShow/
├── Program.cs
├── the_truman_show_script.txt
├── questions.txt
├── answers.txt       
├── LICENSE (GPL-3.0)
└── README.md
```

## Contributing

1. Fork the repo  
2. Make changes  
3. Submit a pull request

## License

GPL‑3.0 License.
