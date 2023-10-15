                [![codecov](https://codecov.io/gh/tusan-tsvetkoff/file-diff-in-csharp/graph/badge.svg?token=U7GFT2VE4V)](https://codecov.io/gh/tusan-tsvetkoff/file-diff-in-csharp)

# File Diff CLI tool in C#

**!FOR EDUCATIONAL PURPOUSES ONLY!**

This is a utility command line tool that compares two files and shows the differences between them. It is extremely simple and currently only supports comparing two files line by line, with the ability to turn one file into the other by adding, removing, or changing lines. (*it essentially only removes lines and adds others in their place*)

Entirely inspired by [tsoding](https://github.com/tsoding).

**References:**
- Tsoding's [video](https://www.youtube.com/watch?v=tm60fuF5v54&ab_channel=TsodingDaily)
    - [source code](https://github.com/tsoding/piff)
- [Levenshtein](https://www.youtube.com/redirect?event=video_description&redir_token=QUFFLUhqa3pMRFFjSkpSQVFNQ19uMFZaNkx3ZWwyM3pQQXxBQ3Jtc0tscFBsd3pRbWdkalpSMFFPQ2lkcTVKS3dKNkxRU2pJdkctVlc2Q3hqMF9wWlV3UDV3TVRPaU5aNFNzR2trd21QaVlfM213aXVzQnluaUJ5VG1iMFp4WGVqWlNmdUpSQUtSNFphOHZuY2xQVnlsbUpNQQ&q=https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FLevenshtein_distance&v=tm60fuF5v54)

## Usage

1. Clone the repository
2. Open a terminal in the root directory of the repository
3. Build the project using `dotnet build`
### Pre-requisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/preview/vs2022/) (optional)
- [Visual Studio Code](https://code.visualstudio.com/) (optional)
- Any other IDE or text editor of your choice.

#### Quick Start

- `dotnet run --project .\src\FileDiff.Cli\FileDiff.Cli.csproj diff <fileA> <fileB>`
    - Compares the two files and shows the differences between them.
- `dotnet run --project .\src\FileDiff.Cli\FileDiff.Cli.csproj diff <fileA> <fileB> > file.patch`
    - Compares the two files and saves the differences between them in a patch file.
- `dotnet run --project .\src\FileDiff.Cli\FileDiff.Cli.csproj patch <file.patch> <fileA>`
    - Applies the patch file to the first file and saves the result.

#### Command List

| Command | Arguments                        | Explanation                                                                                                                            |
| ------- | -------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| `diff`  | **[fileA] [fileB]**              | Calculates the Levenshtein distance between the two files and outputs the actions needed to turn `fileA` into `fileB`                  |
|         | **[fileA] [fileB] > file.patch** | Does the same as the previous command, but it also generates a `.patch` file containing the actions needed to turn `fileA` int `fileB` |
| `patch` | **[file.patch] [fileA]**         | Applies the actions inside `.patch` file to `fileA`                                                                                    |
| `list`  |                                  | Lists all available commands                                                                                                           |
| `help`  |                                  | Prints the full usage of the tool                                                                                                      |
|         | Adding a command before `help`   | Shows you the usage of <command>. If the command is not found, it uses the Levenshtein algorithm to suggest a command.                 |
