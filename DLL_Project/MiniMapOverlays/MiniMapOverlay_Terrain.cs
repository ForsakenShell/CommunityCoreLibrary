﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using UnityEngine;
using Verse;

namespace CommunityCoreLibrary.MiniMap
{

	// TODO: While terrainDefs have the color tag, it is left at default (white), and is therefore not useful.
	public class MiniMapOverlay_Terrain : MiniMapOverlay
	{

		#region Constructors

		public MiniMapOverlay_Terrain( MiniMap minimap, MiniMapOverlayDef overlayData ) : base( minimap, overlayData )
		{
		}

		#endregion Constructors

		#region Methods

		public override void Update()
		{
			// update all cells
			for( int i = 0; i < Find.VisibleMap.cellIndices.NumGridCells; i++ )
			{
				// get x,y position from index
				var position = Find.VisibleMap.cellIndices.IndexToCell( i );

				// paint it... brownish?
				texture.SetPixel( position.x, position.z, Find.VisibleMap.terrainGrid.TerrainAt( i ).color );
			}
		}

		#endregion Methods

	}

}
