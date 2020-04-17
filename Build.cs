using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxjjyx.Common;

namespace xxjjyx
{
    /// <summary>
    /// 建筑类
    /// </summary>
    public class Build
    {
        int _type;
        string _name;
        string _desc;
        Dictionary<string, int> _productionResources;//产出资源
        //bool _isBuilding;
        int _buildCastTime;
        int _buildProgress;
        Dictionary<string, int> _buildCastResources;
        District _setDist;
        Dictionary<string, int> _uphold;
        int _prepositionType;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public int Type { get => _type; set => _type = value; }
        /// <summary>
        /// 建筑名称
        /// </summary>
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// 建筑说明
        /// </summary>
        public string Desc { get => _desc; set => _desc = value; }
        /// <summary>
        /// 生产资源
        /// </summary>
        public Dictionary<string, int> ProductionResources { get => _productionResources; set => _productionResources = value; }
        /// <summary>
        /// 是否在建
        /// </summary>
        //public bool IsBuilding { get => _isBuilding; set => _isBuilding = value; }
        /// <summary>
        /// 建筑消耗时间
        /// </summary>
        public int BuildCastTime { get => _buildCastTime; set => _buildCastTime = value; }
        /// <summary>
        /// 建筑进度
        /// </summary>
        public int BuildProgress { get => _buildProgress; set => _buildProgress = value; }
        /// <summary>
        /// 建设花费资源
        /// </summary>
        public Dictionary<string, int> BuildCastResources { get => _buildCastResources; set => _buildCastResources = value; }
        /// <summary>
        /// 坐落区划
        /// </summary>
        public District SetDist { get => _setDist; set => _setDist = value; }
        /// <summary>
        /// 维护费用
        /// </summary>
        public Dictionary<string, int> Uphold { get => _uphold; set => _uphold = value; }
        /// <summary>
        /// 前置建筑类型
        /// </summary>
        public int PrepositionType { get => _prepositionType; set => _prepositionType = value; }

        public Build(int type/*, District setDist*/)
        {
            //根据类型，构筑建筑
            this.Type = type;
            //this.SetDist = setDist;
            SetBuild();
        }

        void SetBuild()
        {
            this.Name = Globle.Split(BuildInfo.Default.Name)[Type];
            this.Desc = Globle.Split(BuildInfo.Default.Desc)[Type];
            this.BuildCastTime = int.Parse(Globle.Split(BuildInfo.Default.BuildCastTime)[Type]);
            this.BuildCastResources = Resource.GetResDic(Globle.Split(BuildInfo.Default.BuildCastResources)[Type]);
            this.ProductionResources = Resource.GetResDic(Globle.Split(BuildInfo.Default.ProductionResources)[Type]);
            this.Uphold = Resource.GetResDic(Globle.Split(BuildInfo.Default.Uphold)[Type]);
            this.PrepositionType = int.Parse(Globle.Split(BuildInfo.Default.PrepositionType)[Type]);
        }
    }
}
