using System;

class Program
{
    static void Main()
    {
        string inputFile = "C:\\coding\\a.txt";
        string outputFile = "C:\\coding\\b.txt";

        while (true)
        {
            // 파일에서 내용을 읽음
            string[] lines;
            try
            {
                lines = File.ReadAllLines(inputFile); // 입력 파일 내용을 줄 단위로 읽음
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{inputFile} 파일을 찾을 수 없습니다."); // 파일이 없을 경우 예외 처리
                return;
            }

            // 사용자 명령 처리
            Console.Write("Enter command ( /0, /1, /2, /3, /4, /a, /d, 나가기)"); // 명령 방법 /0~4/a 혹은 /0~4/d
            string command = Console.ReadLine().ToLower(); // 사용자에게 명령을 입력받아 소문자로 변환

            if (command == "나가기")
            {
                Console.WriteLine("프로그램을 종료합니다.");
                break; // 사용자가 '나가기'를 입력하면 프로그램 종료
            }

            ProcessCommand(lines, command, outputFile); // 명령을 처리하는 함수 호출
        }
    }

    static void ProcessCommand(string[] lines, string command, string outputFile)
    {
        // 숫자 부분 및 명령 유형 추출
        string[] commandParts = command.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length == 2 && int.TryParse(commandParts[0], out int columnIndex))
        {
            // 명령이 올바르게 입력되었고, 숫자 부분과 명령 타입 추출
            string commandType = commandParts[1].Trim();

            if (columnIndex >= 0 && columnIndex < lines[0].Split(',').Length)
            {
                // 유효한 열 인덱스인 경우
                string[] selectedColumn = lines.Select(line => line.Split(',')[columnIndex].Trim()).ToArray();

                if (commandType == "a" || commandType == "d")
                {
                    // 명령 타입이 'a' 또는 'd' 일 경우
                    Array.Sort(selectedColumn, new CustomComparer());

                    if (commandType == "a")
                    {
                        // 명령 타입이 'a' (정렬) 일 경우
                        Console.WriteLine($"오름차순으로 정렬된 결과: {string.Join(" ", selectedColumn)}");
                        Console.WriteLine($"오름차순으로 정렬된 결과가 {outputFile}에 저장되었습니다.");
                    }
                    else
                    {
                        // 명령 타입이 'd' (역순 정렬) 일 경우
                        Array.Reverse(selectedColumn);
                        Console.WriteLine($"내림차순으로 정렬된 결과: {string.Join(" ", selectedColumn)}");
                        Console.WriteLine($"내림으로 정렬된 결과가 {outputFile}에 저장되었습니다.");
                    }

                    // 정렬된 결과를 파일에 쓰기
                    File.WriteAllText(outputFile, string.Join(" ", selectedColumn));
                }
                else
                {
                    Console.WriteLine("Error: 올바른 명령을 입력하세요.");
                }
            }
            else
            {
                Console.WriteLine("Error: 유효하지 않은 열 인덱스입니다.");
            }
        }
        else
        {
            Console.WriteLine("Error: 올바른 명령을 입력하세요.");
        }
    }

    public class CustomComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            bool isXNumeric = int.TryParse(x, out int xNum);
            bool isYNumeric = int.TryParse(y, out int yNum);

            if (isXNumeric && isYNumeric)
            {
                // 둘 다 숫자면 숫자 값을 기준으로 비교
                return xNum.CompareTo(yNum);
            }

            // 비교대상이 하나는 숫자이고 다른 하나는 숫자가 아닌 경우 숫자를 더 작게 고려
            if (isXNumeric)
            {
                return -1;
            }
            else if (isYNumeric)
            {
                return 1;
            }

            // 둘 다 숫자거나 문자일때 길이를 기준으로 비교
            return x.Length.CompareTo(y.Length);
        }
    }
}