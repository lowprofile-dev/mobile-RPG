using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DunGen.Analysis
{
	public class GenerationAnalysis
	{
        public int TargetIterationCount { get; private set; }
		public int IterationCount { get; private set; }

		public NumberSetData MainPathRoomCount { get; private set; }
		public NumberSetData BranchPathRoomCount { get; private set; }
		public NumberSetData TotalRoomCount { get; private set; }
		public NumberSetData MaxBranchDepth { get; private set; }
		public NumberSetData TotalRetries { get; private set; }
		
		public NumberSetData PreProcessTime { get; private set; }
		public NumberSetData MainPathGenerationTime { get; private set; }
		public NumberSetData BranchPathGenerationTime { get; private set; }
		public NumberSetData PostProcessTime { get; private set; }
		public NumberSetData TotalTime { get; private set; }

		public float AnalysisTime { get; private set; }
		public int SuccessCount { get; private set; }
		public float SuccessPercentage { get { return (SuccessCount / (float)TargetIterationCount) * 100; } }

		private readonly List<GenerationStats> statsSet = new List<GenerationStats>();


        public GenerationAnalysis(int targetIterationCount)
        {
            TargetIterationCount = targetIterationCount;
        }

		public void Clear()
		{
			IterationCount = 0;
			AnalysisTime = 0;
			SuccessCount = 0;
			statsSet.Clear();
		}

		public void Add(GenerationStats stats)
		{
			statsSet.Add(stats.Clone());
			AnalysisTime += stats.TotalTime;
			IterationCount++;
		}

		public void IncrementSuccessCount()
		{
			SuccessCount++;
		}

		public void Analyze()
		{
			MainPathRoomCount = new NumberSetData(statsSet.Select(x => (float)x.MainPathRoomCount));
			BranchPathRoomCount = new NumberSetData(statsSet.Select(x => (float)x.BranchPathRoomCount));
			TotalRoomCount = new NumberSetData(statsSet.Select(x => (float)x.TotalRoomCount));
			MaxBranchDepth = new NumberSetData(statsSet.Select(x => (float)x.MaxBranchDepth));
			TotalRetries = new NumberSetData(statsSet.Select(x => (float)x.TotalRetries));

			PreProcessTime = new NumberSetData(statsSet.Select(x => x.PreProcessTime));
			MainPathGenerationTime = new NumberSetData(statsSet.Select(x => x.MainPathGenerationTime));
			BranchPathGenerationTime = new NumberSetData(statsSet.Select(x => x.BranchPathGenerationTime));
			PostProcessTime = new NumberSetData(statsSet.Select(x => x.PostProcessTime));
			TotalTime = new NumberSetData(statsSet.Select(x => x.TotalTime));
		}
	}
}

