using System;

public class Noise
{

	Octave[] octaves;
	double[] frequencys;
	double[] amplitudes;

	double persistence;

	public Noise(int largestFeature, double persistence, int seed)
	{
		this.persistence = persistence;

		//recieves a number (eg 128) and calculates what power of 2 it is (eg 2^7)
		int numberOfOctaves = (int)Math.Ceiling(Math.Log10(largestFeature) / Math.Log10(2));

		octaves = new Octave[numberOfOctaves];
		frequencys = new double[numberOfOctaves];
		amplitudes = new double[numberOfOctaves];

		System.Random rnd = new System.Random(seed);

		for (int i = 0; i < numberOfOctaves; i++)
		{
			octaves[i] = new Octave(rnd.Next());

			frequencys[i] = Math.Pow(2, i);
			amplitudes[i] = Math.Pow(persistence, octaves.Length - i);
		}

	}


	public double GetNoise(int x, int y)
	{

		double result = 0;

		for (int i = 0; i < octaves.Length; i++)
		{
			//double frequency = Math.pow(2,i);
			//double amplitude = Math.pow(persistence,octaves.length-i);

			result = result + octaves[i].noise(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
		}


		return result;

	}

	public double GetNoise(int x, int y, int z)
	{

		double result = 0;

		for (int i = 0; i < octaves.Length; i++)
		{
			double frequency = Math.Pow(2, i);
			double amplitude = Math.Pow(persistence, octaves.Length - i);

			result = result + octaves[i].noise(x / frequency, y / frequency, z / frequency) * amplitude;
		}


		return result;

	}
}