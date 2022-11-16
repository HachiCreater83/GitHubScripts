#pragma once
#include <DxLib.h>

class 
{
public:
	int White;
	int Black;
	int Green;
	int Red;
	int Blue;

	void Read()
	{
		White = GetColor(255, 255, 255);
		Black=GetColor(0, 0, 0);
		Green= GetColor(0, 255, 0);
		Red= GetColor(255, 0, 0);
		Blue= GetColor(0, 0, 255);
	}

private:
}Color;

class
{
public:
	int colorCount[50 + 1];

	void Read()
	{
		for (int i = 0; i < 50; i = i++) 
		{
			colorCount[i] = CreateFontToHandle("Ms ƒSƒVƒbƒN", i, 6, DX_FONTTYPE_NORMAL);
		}
	}
private:
}Font;

//key info
int Key[256];
//key Function
int GetKey()
{
	char allkey[256];
	GetHitKeyStateAll(allkey);
	for (int i = 0; i < 256; i = i++) {
		if (allkey[i] == 1) {
			Key[i] = Key[i]++;
		}
		else if (allkey[i]==0)
		{
			Key[i] = 0;
		}
	}
	return 0;
}

