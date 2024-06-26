﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace PdfPlus
{
    public class DataSet: Element
    {
        #region members

        protected List<double> values = new List<double>();
        protected List<string> contents = new List<string>();
        protected List<Sd.Color> colors = new List<Sd.Color>();
        protected string title = string.Empty;
        protected bool hasTitle = false;
        protected bool hasGraphics = false;
        protected bool isNumeric = false;

        public enum LabelAlignments { None, Above, End, Middle, Start};
        protected LabelAlignments labelAlignment = LabelAlignments.None;

        #endregion

        #region constructors

        public DataSet(List<double> values):base()
        {
            this.elementType = ElementTypes.Data;
            this.values = values;
            this.Graphic.Stroke = Sd.Color.Black;
            this.Graphic.Weight = 1;
            this.isNumeric = true;
        }

        public DataSet(List<string> contents) : base()
        {
            this.elementType = ElementTypes.Data;
            this.contents = contents;

            List<double> vals = new List<double>();
            foreach (string text in contents)if (double.TryParse(text, out double val)) vals.Add(val);
            if (contents.Count == vals.Count)
            {
                this.values = vals;
                this.isNumeric = true;
            }

            this.Graphic.Stroke = Sd.Color.Black;
            this.Graphic.Weight = 1;
        }

        public DataSet(List<double> values, List<string> labels) : base()
        {
            this.elementType = ElementTypes.Data;
            this.values = values;
            this.Graphic.Stroke = Sd.Color.Black;
            this.Graphic.Weight = 1;
            this.isNumeric = true;
        }

        public DataSet(DataSet dataSet) : base()
        {
            this.elementType = dataSet.elementType;
            this.title = dataSet.title;
            this.hasTitle = dataSet.hasTitle;
            this.Graphic = new Graphic(dataSet.Graphic);
            this.Font = new Font(dataSet.Font);
            this.labelAlignment = dataSet.labelAlignment;
            foreach (double v in dataSet.values) this.values.Add(v);
            foreach (string s in dataSet.contents) this.contents.Add(s);
            foreach (Sd.Color c in dataSet.colors) this.colors.Add(c);
            this.isNumeric = dataSet.isNumeric;
        }

        #endregion

        #region properties

        public virtual bool IsNumeric
        {
            get { return this.isNumeric; }
        }

        public virtual List<string> Contents
        {
            get { return this.contents; }
        }

        public virtual List<double> Values
        {
            get { return this.values; }
        }

        public virtual bool HasColors
        {
            get { return this.colors.Count > 0; }
        }

        public virtual List<Sd.Color> Colors
        {
            get 
            {
                List<Sd.Color> output = colors;
                output.AddRange(Enumerable.Repeat(Sd.Color.Black,this.contents.Count-colors.Count));
                return output; 
            }
            set
            {
                this.colors = value;
                int c = this.colors.Count;
                for(int i = c; i < this.values.Count; i++)
                {
                    this.colors.Add(this.colors[c - 1]);
                }
            }
        }

        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                hasTitle = true;
            }
        }

        public virtual LabelAlignments LabelAlignment
        {
            get { return this.labelAlignment; }
            set { this.labelAlignment = value; }
        }

        public virtual bool LabelData
        {
            get { return this.labelAlignment != LabelAlignments.None; }
        }

        #endregion

        #region methods

        public void SetColors(Sd.Color color)
        {
            this.colors = new List<Sd.Color>();
            for (int i = 0; i < this.Contents.Count; i++) this.colors.Add(color);
        }

        public void SetColors(List<Sd.Color> colors)
        {
            int c = colors.Count;
            this.colors = new List<Sd.Color>();
            for (int i = 0; i < this.Contents.Count; i++) this.colors.Add(colors[i%c]);
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            if (this.IsNumeric)
            {
                return "DataSet | [" + this.contents.Count + "](Numbers)";
            }
            else
            {
                return "DataSet | [" + this.contents.Count + "](Text)";
            }
        }

        #endregion

    }
}
