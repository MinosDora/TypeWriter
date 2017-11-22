using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter
{
    public bool IsTyping { get; private set; }

    public bool IsFastTyper { get; set; }

    private StringBuilder stringBuilder = new StringBuilder();
    private Stack<char> typeStack = new Stack<char>();
    private StringBuilder typeStringBuildertemp = new StringBuilder();
    private StringBuilder typeStringBuilder = new StringBuilder();

    public IEnumerator TypeWriterString(string content, float typerSpaceTime = .05f, Action<string> showContentAction = null)
    {
        IsTyping = true;
        IsFastTyper = false;
        stringBuilder.Remove(0, stringBuilder.Length);
        typeStack.Clear();
        char[] contentCharArray = content.ToCharArray();
        for (int i = 0; i < contentCharArray.Length; i++)
        {
            if (contentCharArray[i] == '<' && i <= contentCharArray.Length - 2)
            {
                if (contentCharArray[i + 1] != '/')
                {
                    for (int j = i; j < contentCharArray.Length; j++)
                    {
                        stringBuilder.Append(contentCharArray[j]);
                        typeStack.Push(contentCharArray[j]);
                        if (contentCharArray[j] == '>')
                        {
                            i = j;
                            break;
                        }
                    }
                    continue;
                }
                else
                {
                    for (int j = typeStack.Count - 1; j >= 0; j--)
                    {
                        if (typeStack.Pop() == '<')
                        {
                            break;
                        }
                    }
                    for (int j = i; j < contentCharArray.Length; j++)
                    {
                        stringBuilder.Append(contentCharArray[j]);
                        if (contentCharArray[j] == '>')
                        {
                            i = j;
                            break;
                        }
                    }
                    continue;
                }
            }
            else
            {
                if (typeStack.Count > 0)
                {
                    typeStringBuildertemp.Remove(0, typeStringBuildertemp.Length);
                    typeStringBuilder.Remove(0, typeStringBuilder.Length);
                    foreach (var item in typeStack)
                    {
                        typeStringBuildertemp.Append(item);
                    }
                    for (int j = typeStringBuildertemp.Length - 1; j >= 0; j--)
                    {
                        typeStringBuilder.Append(typeStringBuildertemp[j]);
                    }
                }
                stringBuilder.Append(contentCharArray[i]);
                if (showContentAction != null)
                {
                    string showContentStr = stringBuilder.ToString();
                    if (typeStack.Count > 0)
                    {
                        string[] typeStr = typeStringBuilder.ToString().Split(new char[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = typeStr.Length - 1; j >= 0; j--)
                        {
                            if (typeStr[j].IndexOf("<color") >= 0)
                            {
                                showContentStr += "</color>";
                            }
                            else if (typeStr[j].IndexOf("<size") >= 0)
                            {
                                showContentStr += "</size>";
                            }
                            else if (typeStr[j].IndexOf("<i") >= 0)
                            {
                                showContentStr += "</i>";
                            }
                            else if (typeStr[j].IndexOf("<b") >= 0)
                            {
                                showContentStr += "</b>";
                            }
                        }
                    }
                    showContentAction(showContentStr);
                }
                if (!IsFastTyper)
                {
                    yield return new WaitForSeconds(typerSpaceTime);
                }
            }
        }
        IsTyping = false;
    }
}