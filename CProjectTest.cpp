// CProjectTest.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//#include　はライブラリを使えるようにする

#include <iostream>
#include <string>

using namespace std;

//メイン関数、名前通り本体として実際に実行される部分
// main関数で宣言した変数は、main関数外でその変数は使えない
//cout << というのが、画面出力を行うコード
//\nは改行を意味しています
//標準ライブラリのメソッドを使うときは頭にstd::をつける
//iostreamは入出力ライブラリ、stringは文字列ライブラリです。
//+-*÷は演算子(オペレーター)と呼ばれる
//+=,-=,*=,/=が代入演算子
//代入演算子は演算を省略して書く方法で、例えばx += 3;ではｘに３を足した結果をｘに代入します
//bool型は変数型の一種で、整数型のintや浮動小数点型のdouble、文字列型のstringに並ぶ一つの型です
//bool型は少し特殊で、TrueとFalseの２つしか入れられません
//else if(x > 10){　この行で出てくるelse if文は、if文が実行されなかった場合にだけ実行されるif文です
//else if文はif文がないと存在できません
//elseは「そうでなければ」を意味するので、else ifは「if でなければ」という意味に
//文字列を操作するstring
//using namespace stdを書くことで、これより下のプログラムではstdを省略して書くことができます
//stdの名前空間を使用するという意味です
//std::cout　というのは、標準ライブラリ（std）内のcoutを使用するという意味

/*int main()
{
    int x;
    x = 3;
    int y = 5;
    int z;

    z = x + y;

    std::cout << z << "\n";
    std::cout << x - y << "\n";
    std::cout << x * y << "\n";
    std::cout << x / y << "\n";
    std::cout << x % y << "\n";

    double d = 3.14;

    std::cout << d / x << "\n";

    char c = 'a';

    std::string message = "I love";
    std::string message2 = " Japan";

    std::cout << message + message2;
}
*/

/* int main()
{
    int x = 3;

    x += 3;
    x -= 3;
    x *= 3;
    x /= 3;

    std::cout << x << "\n";
    std::cout << ++x << "\n";
    std::cout << ++x << "\n";
    std::cout << x << "\n";
    std::cout << x++ << "\n";
    std::cout << x++ << "\n";
}
*/

/*int main()
{
    bool is_ok = false;
    is_ok = true;

    if (is_ok) {
        std::cout << "is_ok = true" << "\n";
    }

    int x = 30;

    if (x > 100) {
        std::cout << "x > 100" << "\n";
    }
    else if (x > 10) {
        std::cout << "x > 10" << "\n";
    }
    else {
        if (x == 10) std::cout << "x は 10 です" << "\n";
        std::cout << " x <= 10" << "\n";
    }

    int y = 50;

    if (x > 10 && y > 10) {
        std::cout << "xもyも10以上" << "\n";
    }

}
*/

int main()
{
    std::cout << "Hello world\n";
    cout << "Hello world\n";

    std::cout << "Hello world" << std::endl;
    cout << "Hello world" << endl;

    std::string message = "I love";
    string message2 = "I love";

    {
        int x = 10;
        cout << x << endl;
    }

    if (true) {
        int x = 10;
        cout << x << endl;
    }

    cin >> message;
    cout << message;
}