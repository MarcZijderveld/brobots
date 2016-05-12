#include "game.h"
#include "surface.h"

using namespace Tmpl8;
int smoothed;
// -----------------------------------------------------------
// Scale a color (range: 0..128, where 128 is 100%)
// -----------------------------------------------------------
inline unsigned int ScaleColor( unsigned int orig, char scale )
{
	const Pixel rb = ((scale * (orig & ((255 << 16) + 255))) >> 7) & ((255 << 16) + 255);
	const Pixel g = ((scale * (orig & (255 << 8))) >> 7) & (255 << 8);
	return (Pixel)rb + g;
}

// -----------------------------------------------------------
// Draw a glass ball using fake reflection & refraction
// -----------------------------------------------------------
void Game::DrawBall( int bx, int by )
{
	Pixel* dst = m_Surface->GetBuffer() + bx + by * m_Surface->GetPitch();
	Pixel* src = m_Image->GetBuffer();
	for ( int x = 0; x < 128; x++ )
	{
		for ( int y = 0; y < 128; y++ )
		{
			float dx = (float)(x - 64);
			float dy = (float)(y - 64);
			int dist = (int)sqrt( dx * dx + dy * dy );
			if (dist < 64)
			{
				int xoffs = (int)((dist / 2 + 10) * sin( (float)(x - 50) / 40.0 ) );
				int yoffs = (int)((dist / 2 + 10) * sin( (float)(y - 50) / 40.0 ) );
				int u1 = (((bx + x) - 4 * xoffs) + SCRWIDTH) % SCRWIDTH;
				int v1 = (((by + y) - 4 * yoffs) + SCRHEIGHT) % SCRHEIGHT;
				int u2 = (((bx + x) + 2 * xoffs) + SCRWIDTH) % SCRWIDTH;
				int v2 = (((by + y) + 2 * yoffs) + SCRHEIGHT) % SCRHEIGHT;
				Pixel refl = src[u1 + v1 * m_Image->GetPitch()];
				Pixel refr = src[u2 + v2 * m_Image->GetPitch()];
				int reflscale = (int)(63.0f - 0.015f * (1 - dist) * (1 - dist));
				int refrscale = (int)(0.015f * (1 - dist) * (1 - dist));
				dst[x + y * m_Surface->GetPitch()] = ScaleColor( refl, 41 - (int)(reflscale * 0.5f) ) + ScaleColor( refr, 63 - refrscale );
				float3 L = Normalize( float3( 60, -90, 85 ) );
				float3 p = float3( (x - 64) / 64.0f, (y - 64) / 64.0f, 0 );
				p.z = sqrt( 1.0f - (p.x * p.x + p.y * p.y) );
				float d = min( 1, max( 0, Dot( L, p ) ) );
				d = powf( d, 140 );
				Pixel highlight = ((int)(d * 255.0f) << 16) + ((int)(d * 255.0f) << 8) + (int)(d * 255.0f);
				dst[x + y * m_Surface->GetPitch()] = AddBlend( dst[x + y * m_Surface->GetPitch()], highlight );
			}
		}
	}
}

// -----------------------------------------------------------
// Initialize the game
// -----------------------------------------------------------
void Game::Init()
{
	//TODO remove smoothed
	smoothed = 0;
	m_Image = new Surface( "testdata/mountains.png" );
	m_Backdrop = new Surface( "testdata/mountains.png" );
	
	Pixel* src = m_Backdrop->GetBuffer();
	unsigned int count = m_Backdrop->GetPitch() * m_Backdrop->GetHeight();
	for ( unsigned int i = 0; i < count; i++ )
	{
		src[i] = ScaleColor( src[i], 20 );
		int grey = src[i] & 255;
		src[i] = grey + (grey << 8) + (grey << 16);
	}

	m_BallX = 100;
	m_BallY = 100;
	m_VX = 1.6f;
	m_VY = 0;
}

// -----------------------------------------------------------
// Draw the backdrop and make it a bit darker
// -----------------------------------------------------------
void Game::DrawBackdrop()
{
	///m_Backdrop->CopyTo( m_Surface, 0, 0 );
		m_Image->CopyTo( m_Surface, 0, 0 );
	Pixel* src = m_Surface->GetBuffer();
	unsigned int count = m_Surface->GetPitch() * m_Surface->GetHeight();
	for ( unsigned int i = 0; i < count; i++ )
	{
		src[i] = ScaleColor( src[i], 20 );
		int grey = src[i] & 255;
		src[i] = grey + (grey << 8) + (grey << 16);
	}
}

// -----------------------------------------------------------
// Main game tick function
// -----------------------------------------------------------
void Game::Tick( float a_DT )
{
	//TODO remove testcode
	TimerRDTSC timer;
	
	timer.Start();

	m_Surface->Clear( 0 );
	DrawBackdrop();

	timer.Stop();
	
	if(smoothed == 0)
		smoothed = timer.Interval();
	else
		smoothed = 0.99f * smoothed +  0.01f * timer.Interval();
	char t[128];
	sprintf( t, "(%i ticks)", smoothed);
	m_Surface->Print( t, 2, 12, 0xff4444 );

	DrawBall( (int)m_BallX, (int)m_BallY );
	m_BallY += m_VY;
	m_BallX += m_VX;
	m_VY += 0.2f;
	if (m_BallY > (SCRHEIGHT - 128))
	{
		m_BallY = SCRHEIGHT - 128;
		m_VY = -0.96f * m_VY;
	}
	if (m_BallX > (SCRWIDTH - 138))
	{
		m_BallX = SCRWIDTH - 138;
		m_VX = -m_VX;
	}
	if (m_BallX < 10)
	{
		m_BallX = 10;
		m_VX = -m_VX;
	}
}