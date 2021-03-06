﻿using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRaskEvaltionSystem.RouteAnalysis
{
    /// <summary>
    /// 路线决策实现类1
    /// </summary>
     class RouteDecideClass:IRouteDecide
    {
        IRouteConfig routeConfig = null;
        public RouteDecideClass(IRouteConfig config)
        {
            this.routeConfig = config;
        }
        /// <summary>
        /// 查询点缓冲区以内的要素
        /// </summary>
        /// <param name="point"></param>
        /// <param name="map"></param>
        /// <param name="layer"></param>
        /// <param name="buffer_distance"></param>
        /// <returns></returns>
        public IFeature QuerySingleFeatureByPoint(IPoint  point,IMap map,IFeatureLayer layer,double buffer_distance)
        {
          //为了安全在此判断是否map中是否已经包括layer
            bool isContain=false;
            for(int i=0;i<map.LayerCount;i++)
            {
                if(map.get_Layer(i)==layer)
                {
                    isContain=true;
                    break;
                }
            }
            if(!isContain) 
            {
                throw new Exception("Map中不包括该图层");
            }
            ITopologicalOperator pTopOperator = point as ITopologicalOperator;
            IGeometry pGeometry = pTopOperator.Buffer(buffer_distance);
            IIdentify pIdentity = layer as IIdentify;
           IArray pArray= pIdentity.Identify(pGeometry);
            IFeature pFeature = null;
            if(pArray!=null)
            {
                 pFeature = (pArray.get_Element(0) as IRowIdentifyObject).Row as IFeature;
            }
            return pFeature;
        }

        /// <summary>
        /// 查询绕行路线 0代表没有查询到
        /// </summary>
        /// <param name="point"></param>
        /// <param name="featureLayer"></param>
        /// <param name="rightPoint"></param>
        /// <returns></returns>
        public string QueryTheRoute(IPoint point,IFeatureLayer featureLayer,ref IPoint rightPoint)
        {
            //查询离所点击的点最近的元素
            IFeature feature = QueryTheRightFeatureByPoint(point, featureLayer, ref rightPoint);
            if (feature == null)
            {
                return null;
            }
            int index = feature.Fields.FindField("OBJECTID");
           int objectID=(int) feature.get_Value(index);
            return routeConfig.QueryGoodRouteIndex(objectID);
        }
        /// <summary>
        /// 查询配置文件中线要素集合中离点最近的元素
        /// </summary>
        /// <param name="point"></param>
        /// <param name="map"></param>
        /// <param name="featureLayer"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private IFeature QueryTheRightFeatureByPoint(IPoint point, IFeatureLayer featureLayer, ref IPoint rightPoint)
        {
            List<IFeature> featuers = QueryAllFeatureInConfig(featureLayer);
            IFeature feature = null;
            double resultDistance = 9999999999;
            foreach (IFeature value in featuers)
            {
                double tempValue=0;
                int disNum=0;
                IPoint tempPoint=DistanceUtil.GetNearestLine(value.Shape as  IPolyline, point, ref tempValue, ref disNum);
                if(tempValue<resultDistance)
                {
                    resultDistance = tempValue;
                    feature = value;
                    rightPoint = tempPoint;
                }
            }
            return feature;
        }
        /// <summary>
        /// 查询所有配置文件中的要素
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public List<IFeature> QueryAllFeatureInConfig(IFeatureLayer layer)
        {
            List<IFeature> queryFeaturers = new List<IFeature>();
            IFeatureClass featureClass=layer.FeatureClass;
            Dictionary<int, string> queryObjectIDS=routeConfig.QueryIndexs;
            foreach(var v in queryObjectIDS)
            {
                IFeature feature = QuerySingleFeature(featureClass, v.Key);
                if(feature!=null)
                {
                    queryFeaturers.Add(feature);
                }
            }
            return queryFeaturers;
        }
        /// <summary>
        /// 根据OBJECTID查询单个要素
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="objecID"></param>
        /// <returns></returns>
        public IFeature QuerySingleFeature(IFeatureClass featureClass, int objecID)
        {
            IQueryFilter2 queryFilter2 = new QueryFilterClass();
            queryFilter2.WhereClause = "OBJECTID = " + objecID.ToString();
            //Using a query filter to search a feature class:
            IFeatureCursor featureCursor = featureClass.Search(queryFilter2, false);
            return featureCursor.NextFeature();
        }


        public bool QueryTheRoue(IPoint breakPoint, ESRI.ArcGIS.Controls.AxMapControl mapControl, IFeatureLayer featureLayer, string dbPath, string featureSetName, string ndsName, ref IPoint rightPoint)
        {
            throw new NotImplementedException();
        }
    }
}
