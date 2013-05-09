using System;
using System.Collections.Generic;
using System.Reflection;

namespace FantasyScape {
	class ResourceManager {
		public static void Load() {
			List<Resource> Resources = GetResources();

			foreach (Resource resource in Resources) {
				Console.WriteLine("Loading " + resource.GetType().Name);
				resource.Load();
			}
		}

		static List<Resource> GetResources() {
			List<Type> Types = new List<Type>(Assembly.GetAssembly(typeof(Game)).GetTypes());
			List<Resource> ret = new List<Resource>();
			foreach (Type type in Types) {
				List<Type> Interfaces = new List<Type>(type.GetInterfaces());
				if (Interfaces.Contains(typeof(Resource))) {
					object instance = GetInstance(type);
					if (instance != null) {
						ret.Add((Resource)instance);
					} else {
						throw new Exception("Resource type '" + type.Name + "' does not have an empty constructor");
					}
				}
			}

			return ret;
		}

		private static object GetInstance(Type type) {
			if (type.GetConstructor(new Type[0]) != null) {
				return type.GetConstructor(new Type[0]).Invoke(new object[0]);
			} else {
				return null;
			}
		}
	}
}
