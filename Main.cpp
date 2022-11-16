
#define WIN_MAX_X 660
#define WIN_MAX_Y 450
#define WIN_POS_X 0
#define WIN_POS_Y 0
#define POS_MAX_X 6600
#define POS_MAX_Y 450
#define CELL 30
#define STAGE_MAX_X 6600/30
#define STAGE_MAX_Y 450/30
#define _CRT_SECURE_NO_WARNINGS


//Title Scene seviching
enum MENU
{
	MENU_00_Title,
	MENU_01_Action,
};

int Scene = MENU::MENU_00_Title;

#include "DxLib.h"
#include "Sub.h"
#include "Pic.h"
#include "Title.h"
#include "Stage.h"
#include "Action.h"

int WINAPI WinMain(
	_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstanece,
	_In_ LPSTR lpCmdLine,
	_In_ int nShowCmd
)
{
	ChangeWindowMode(TRUE);
	if (DxLib_Init() == -1)	// ＤＸライブラリ初期化処理
	{
		return -1;				// エラーが起きたら直ちに終了
	}

	//Window init
	SetWindowInitPosition(WIN_POS_X, WIN_POS_Y);
	SetWindowText("Action Game");
	SetGraphMode(WIN_MAX_X, WIN_MAX_Y, 32);
	SetBackgroundColor(255, 255, 255);
	SetDrawScreen(DX_SCREEN_BACK);

	//Read
	Color.Read();
	Font.Read();
	Pic.Read();
	Stage.Read();

	//Case Select
	while (ScreenFlip() == 0 &&
		ClearDrawScreen() == 0 &&
		ProcessMessage() == 0 &&
		GetKey() == 0 &&
		Key[KEY_INPUT_ESCAPE] == 0)
	{
		switch (Scene)
		{
		case MENU::MENU_00_Title:
			Title.Out();
				break;
		case MENU::MENU_01_Action:
			Stage.Out();
				break;
		}
	}

	WaitKey();					// キーの入力待ち((7-3)『WaitKey』を使用)

	DxLib_End();				// ＤＸライブラリ使用の終了処理
	return 0;					// ソフトの終了
}
