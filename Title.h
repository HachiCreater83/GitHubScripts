#pragma once

//�^�C�g����ʂ̏o�͂ɗp����N���X
class TITLE
{
public:
	void Out() 
	{
		DrawGraph(0, 0, Pic.Title, TRUE);
		DrawFormatStringToHandle(150, 300, Color.Black, Font.colorCount[30], "Press Enter Key.");
		if (Key[KEY_INPUT_NUMPADENTER] == 1)
		{
			//Music.Play
			Scene = MENU::MENU_01_Action;
		}
	}
private:
};

TITLE Title;