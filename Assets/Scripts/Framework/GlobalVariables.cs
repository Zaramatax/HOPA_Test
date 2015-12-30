using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework {
    public abstract class Variable {
        public abstract object value { get; set;}
        public abstract void SetValue(object value);
        public abstract object GetValue();
    }

    public class Variable<T> : Variable {
        public override object value { get; set;}

        public override void SetValue(object newValue) {
            value = (T) newValue;
        }

        public override object GetValue() {
            return value;
        }
    }

    public static class GlobalVariables {

        static Dictionary<string, Variable> variables = new Dictionary<string,Variable>();

        public static void SetVariable<T>(string name, T value) {
            Variable variable;

            if (!variables.ContainsKey(name)) {
                variable = new Variable<T>();
                variables.Add(name, variable);
            }
            else {
                variable = variables[name];
            }

            variable.SetValue(value);
        }

        public static T GetVariable<T>(string name) {
            if (variables.ContainsKey(name)) {
                return (T) variables[name].GetValue();
            }

            return default(T);
        }
    }
}