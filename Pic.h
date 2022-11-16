#pragma once

//画像やスプライトに用いる型
class
{
public:

	int Title;
	int Back;
	int Block1;
	int Block2;
	int Block3;
	int Enemy;
	int Player;


	void Read()
	{
		Title = LoadGraph("nc261678.png");
		Back = LoadGraph("back.bmp");
		Block1 = LoadGraph("Tex1.bmp");
		Block2 = LoadGraph("Tex1.bmp");
		Block3 = LoadGraph("Tex1.bmp");
		Enemy = LoadGraph("Slim1.png");
		Player = LoadGraph("Hol.png");
	}
private:
} Pic;

