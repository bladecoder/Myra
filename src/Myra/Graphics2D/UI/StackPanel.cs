﻿using Microsoft.Xna.Framework;
using Myra.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Myra.Graphics2D.UI
{
	public abstract class Box: SingleItemContainer<Grid>, IMultipleItemsContainer
	{
		private bool _dirty = true;

		[Browsable(false)]
		[XmlIgnore]
		public abstract Orientation Orientation
		{
			get;
		}

		[Category("Box")]
		[DefaultValue(false)]
		public bool ShowGridLines
		{
			get => InternalChild.ShowGridLines;
			set => InternalChild.ShowGridLines = value;
		}

		[Category("Box")]
		[DefaultValue(0)]
		public int Spacing
		{
			get => Orientation == Orientation.Horizontal ? InternalChild.ColumnSpacing : InternalChild.RowSpacing;
			set
			{
				if (Orientation == Orientation.Horizontal)
				{
					InternalChild.ColumnSpacing = value;
				}
				else
				{
					InternalChild.RowSpacing = value;
				}
			}
		}

		[Browsable(false)]
		public Proportion DefaultProportion
		{
			get => Orientation == Orientation.Horizontal ? InternalChild.DefaultColumnProportion : InternalChild.DefaultRowProportion;
			set
			{
				if (Orientation == Orientation.Horizontal)
				{
					InternalChild.DefaultColumnProportion = value;
				}
				else
				{
					InternalChild.DefaultRowProportion = value;
				}
			}
		}

		[Browsable(false)]
		public ObservableCollection<Proportion> Proportions
		{
			get => Orientation == Orientation.Horizontal ? InternalChild.ColumnsProportions : InternalChild.RowsProportions;
		}

		[Browsable(false)]
		[Content]
		public ObservableCollection<Control> Widgets => InternalChild.Widgets;

		[DefaultValue(HorizontalAlignment.Stretch)]
		public override HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return base.HorizontalAlignment;
			}
			set
			{
				base.HorizontalAlignment = value;
			}
		}

		[DefaultValue(VerticalAlignment.Stretch)]
		public override VerticalAlignment VerticalAlignment
		{
			get
			{
				return base.VerticalAlignment;
			}
			set
			{
				base.VerticalAlignment = value;
			}
		}

		protected Box()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;

			InternalChild = new Grid();
			DefaultProportion = Proportion.BoxDefault;
			Widgets.CollectionChanged += Widgets_CollectionChanged;
		}

		private void UpdateGrid()
		{
			if (!_dirty)
			{
				return;
			}

			var index = 0;
			foreach (var widget in Widgets)
			{
				if (Orientation == Orientation.Horizontal)
				{
					widget.GridColumn = index;
				}
				else
				{
					widget.GridRow = index;
				}

				++index;
			}

			_dirty = false;
		}

		private void Widgets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			_dirty = true;
		}

		protected override Point InternalMeasure(Point availableSize)
		{
			UpdateGrid();

			return base.InternalMeasure(availableSize);
		}

		public override void Arrange()
		{
			UpdateGrid();

			base.Arrange();
		}
	}
}