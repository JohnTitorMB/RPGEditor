using System;
using System.Collections.Generic;
using System.Reflection;

public class SetterAction
{
    public static void Update(MyAction action)
    {
        object entity = RPGManager.Instanse.GetEntity(action.parametres[0]);
        if (entity != null)
        {
            Type type = entity.GetType();
            PropertyInfo[] propertyInfo2 = type.GetProperties();

            PropertyInfo propertyInfo = type.GetProperty((string)action.parametres[1].parameterString);

            if (action.parametres[2].GetParameterType() == typeof(int))
                propertyInfo.SetValue(entity, action.parametres[2].parameterInt);
            else if (action.parametres[2].GetParameterType() == typeof(bool))
                propertyInfo.SetValue(entity, action.parametres[2].parameterBool);
            else if (action.parametres[2].GetParameterType() == typeof(string))
                propertyInfo.SetValue(entity, action.parametres[2].parameterString);
            else if (action.parametres[2].GetParameterType() == typeof(float))
                propertyInfo.SetValue(entity, action.parametres[2].parameterfloat);
            else
            {
                object entityParameter = RPGManager.Instanse.GetEntity(action.parametres[2]);
                propertyInfo.SetValue(entity, entityParameter);
            }
        }

        if (action.nextAction != null)
            action.nextAction.Update();

    }
}