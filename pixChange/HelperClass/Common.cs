﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using pixChange;
using pixChange.HelperClass;

namespace RoadRaskEvaltionSystem.HelperClass
{
    //存储一些公共的变量，数据等
   public class Common
    {
       /// <summary>
       /// 栅格底图的存储路径
       /// </summary>
       public static string rasterPath = @"..\..\Rources\RasterData\";
       /// <summary>
       /// 矢量底图的存储路径
       /// </summary>
       public static string shapPath = @"..\..\Rources\ShapeData\";
       /// <summary>
       /// areaXML保存路径
       /// </summary>
       public static string arexmlPath = @"..\..\Rources\xmlData\AreaXML.xml";
       public
            static string[] hour = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22","23"};

       public static AccessDataBase DBHander = new AccessDataBase();
       /// <summary>
       /// 地图操作工具
       /// </summary>
       public enum CustomTool
       {
           None = 0,
           ZoomIn = 1,
           ZoomOut = 2,
           Pan = 3,
           RuleMeasure = 4,
           AreaMeasure = 5,
           PointSelect = 6,
           RectSelect = 7,
           PolygonSelect = 8,
           CircleSelect = 9,
           NAanalysis = 10,
           StartEditing = 11,
           SelectFeature = 12,
           MoveFeature = 13,
           EditVertex = 14,
           EditUndo = 15,
           EditRedo = 16,
           EditDeleteFeature = 17,
           EditAttribute = 18,
       };
       //地图操作
       //放大
       public void ToolButtonZoomIn(AxMapControl m_mapControl)
       {
           ICommand zoomIn;
           zoomIn = new ControlsMapZoomInTool();
           zoomIn.OnCreate(m_mapControl);
           m_mapControl.CurrentTool = zoomIn as ITool;
         //  m_cTool = CustomTool.ZoomIn;
       }
       //缩小
       public void ToolButtonZoomOut(IMapControl3 m_mapControl)
       {
           ICommand zoomOut;
           zoomOut = new ControlsMapZoomOutTool();
           zoomOut.OnCreate(m_mapControl);
        //   m_cTool = CustomTool.ZoomOut;
           m_mapControl.CurrentTool = zoomOut as ITool;
       }
       //平移
       public void ToolButtonPan(IMapControl3 m_mapControl)
       {
           ICommand pan;
           pan = new ControlsMapPanTool();
           pan.OnCreate(m_mapControl);
         //  m_cTool = CustomTool.Pan;
           m_mapControl.CurrentTool = pan as ITool;
       }
       //全景
       public static void ToolButtonFull(IMapControl3 m_mapControl)
       {
           m_mapControl.Extent = m_mapControl.FullExtent;
       }

       //public static void mapHandel()
       //{
       //    IFeatureLayer pFeatureLayer;
       //    IFeatureClass pFeatureClass;
       //    //新建一个空间过滤器
       //    ISpatialFilter pSpatialFilter;
       //    IQueryFilter pFilter;

       //    IFeatureCursor pCursor;

       //    //定义各种空间类型数据的符号
       //    ISimpleMarkerSymbol simplePointSymbol;
       //    ISimpleFillSymbol simpleFillSymbol;
       //    ISimpleLineSymbol simpleLineSymbol;
       //    //用于闪烁的符号
       //    ISymbol symbol;

       //    IFeature pFeature;
       //    switch (m_cTool)
       //    {
       //        case MainFrom.CustomTool.ZoomIn:
       //        case MainFrom.CustomTool.ZoomOut:
       //            //case CustomTool.Pan:
       //            //Create a point in map coordinates
       //            IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
       //            m_mouseDownPoint = new ESRI.ArcGIS.Geometry.Point();
       //            pPoint.X = e.mapX;
       //            pPoint.Y = e.mapY;
       //            m_mouseDownPoint = pPoint;
       //            m_isMouseDown = true;
       //            break;
       //        case MainFrom.CustomTool.RuleMeasure://测距离
       //            IPolyline plinemeasure;
       //            plinemeasure = (IPolyline)m_mapControl.TrackLine();
       //            ISpatialReferenceFactory spatialReferenceFactory;
       //            spatialReferenceFactory = new SpatialReferenceEnvironment();

       //            IProjectedCoordinateSystem pPCS;
       //            pPCS = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_WGS1984N_AsiaAlbers);
       //            plinemeasure.Project(pPCS);

       //            m_mapControl.MapUnits = esriUnits.esriKilometers;

       //            IGeometry input_geometry;
       //            input_geometry = plinemeasure.FromPoint;
       //            IProximityOperator proOperator = (IProximityOperator)input_geometry;
       //            double check;
       //            check = proOperator.ReturnDistance(plinemeasure.ToPoint);

       //            MessageBox.Show("所测距离为：" + check.ToString("#######.##") + "米");
       //            m_cTool = MainFrom.CustomTool.None;
       //            break;


       //        case MainFrom.CustomTool.RectSelect:
       //            //    pFeatureLayer = (IFeatureLayer)m_mapControl.get_Layer(toolComboBox.SelectedIndex);
       //            pFeatureLayer = (IFeatureLayer)LayerMange.RetuenLayerByLayerNameLayer(m_mapControl, toolComboBox.SelectedItem.ToString());
       //            pFeatureClass = pFeatureLayer.FeatureClass;
       //            IEnvelope pRect = new Envelope() as IEnvelope;
       //            pRect = m_mapControl.TrackRectangle();

       //            //新建一个空间过滤器
       //            pSpatialFilter = new SpatialFilter();
       //            pSpatialFilter.Geometry = pRect;
       //            //依据被选择的要素类的类型不同，设置不同的空间过滤关系
       //            switch (pFeatureClass.ShapeType)
       //            {
       //                case esriGeometryType.esriGeometryPoint:
       //                case esriGeometryType.esriGeometryMultipoint:
       //                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
       //                    break;
       //                case esriGeometryType.esriGeometryPolyline:
       //                case esriGeometryType.esriGeometryLine:
       //                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
       //                    break;
       //                case esriGeometryType.esriGeometryPolygon:
       //                case esriGeometryType.esriGeometryEnvelope:
       //                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
       //                    break;
       //            }


       //            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
       //            pFilter = pSpatialFilter;
       //            //通过空间关系查询
       //            pCursor = pFeatureLayer.Search(pFilter, false);

       //            //定义各种空间类型数据的符号
       //            simplePointSymbol = new SimpleMarkerSymbol();
       //            simpleFillSymbol = new SimpleFillSymbol();
       //            simpleLineSymbol = new SimpleLineSymbol();
       //            simplePointSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
       //            simplePointSymbol.Size = 5;
       //            simplePointSymbol.Color = ColorHelper.GetRGBColor(255, 0, 0);

       //            simpleLineSymbol.Width = 2;
       //            simpleLineSymbol.Color = ColorHelper.GetRGBColor(255, 0, 99);
       //            simpleFillSymbol.Outline = simpleLineSymbol;
       //            simpleFillSymbol.Color = ColorHelper.GetRGBColor(222, 222, 222);
       //            //用于闪烁的符号
       //            pFeature = pCursor.NextFeature();
       //            DataTable pDataTable = createDataTableByLayer(pFeatureLayer as ILayer);
       //            while (pFeature != null)
       //            {
       //                axMapControl1.Map.SelectFeature(pFeatureLayer, pFeature);  //高亮显示
       //                IGeometry pShape;
       //                pShape = pFeature.Shape;
       //                ITable pTable = pFeature as ITable;
       //                DataRow pDataRow = pDataTable.NewRow();
       //                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
       //                {

       //                    if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
       //                    {
       //                        pDataRow[i] = getShapeType(pFeatureLayer as ILayer);
       //                    }
       //                    else if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
       //                    {
       //                        pDataRow[i] = "Element";
       //                    }
       //                    else
       //                    {
       //                        pDataRow[i] = pFeature.get_Value(i);
       //                    }

       //                }
       //                pDataTable.Rows.Add(pDataRow);
       //                switch (pFeatureClass.ShapeType)
       //                {
       //                    case esriGeometryType.esriGeometryPoint:
       //                    case esriGeometryType.esriGeometryMultipoint:
       //                        symbol = (ISymbol)simplePointSymbol;
       //                        m_mapControl.FlashShape(pShape, 5, 100, symbol);
       //                        break;
       //                    case esriGeometryType.esriGeometryPolyline:
       //                    case esriGeometryType.esriGeometryLine:
       //                        symbol = (ISymbol)simpleLineSymbol;
       //                        m_mapControl.FlashShape(pShape, 5, 100, symbol);
       //                        break;
       //                    case esriGeometryType.esriGeometryPolygon:
       //                    case esriGeometryType.esriGeometryEnvelope:
       //                        symbol = (ISymbol)simpleFillSymbol;
       //                        m_mapControl.FlashShape(pShape, 5, 100, symbol);
       //                        break;
       //                }

       //                pFeature = pCursor.NextFeature();
       //            }
       //            m_cTool = MainFrom.CustomTool.None;

       //            ProListView result = new ProListView();
       //            result.showTable(pDataTable);

       //            result.getLayerName(pFeatureLayer.Name);
       //            result.Show();
       //            //可以让选中的区域立即显示出来
       //            MainFrom.m_mapControl.Refresh(esriViewDrawPhase.esriViewGeography, null, null);
       //            break;

       //        case MainFrom.CustomTool.Pan:
       //            pPoint = new ESRI.ArcGIS.Geometry.Point();
       //            pPoint.X = e.mapX;
       //            pPoint.Y = e.mapY;
       //            m_mouseDownPoint = pPoint;
       //            m_isMouseDown = true;
       //            m_focusScreenDisplay = m_mapControl.ActiveView.ScreenDisplay;
       //            m_focusScreenDisplay.PanStart(m_mouseDownPoint);
       //            break;


       //    }
       //}

      
    }
}
