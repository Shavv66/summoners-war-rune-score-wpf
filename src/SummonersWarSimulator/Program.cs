using SimulationApplicationServices;

namespace SummonersWarSimulator
{
	class Program
	{
		static void Main(string[] args)
		{
			SimulationService simulationService = new SimulationService();
			simulationService.RunSimulation();
		}
	}
}
