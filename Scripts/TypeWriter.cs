using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter
{
    //打字机间隔时间
    private float typerSpaceTime = .05f;

    //是否快速打印
    private bool isFastTyper = false;

    //是否正在打印
    private bool isTyperting = false;
    //用于打印的字符串
    private StringBuilder stringBuilder = new StringBuilder();
    //存放类型的堆栈
    private Stack<char> typeStack = new Stack<char>();
    //临时存放类型的字符串
    private StringBuilder typeStringBuildertemp = new StringBuilder();
    //存放类型的字符串
    private StringBuilder typeStringBuilder = new StringBuilder();

    /// <summary>
    /// 打字机实现方法
    /// </summary>
    /// <param name="content">需要打字的字符串</param>
    /// <param name="showContentAction">每打印一个字符的Action，参数为符合富文本格式的当前字符串</param>
    /// <returns></returns>
    public IEnumerator TypeWriterString(string content, Action<string> showContentAction = null)
    {
        isTyperting = true;
        isFastTyper = false;
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
                if (!isFastTyper)
                {
                    yield return new WaitForSeconds(typerSpaceTime);
                }
            }
        }
        isTyperting = false;
    }

    public void SetTyperFast(bool isFastTyper)
    {
        this.isFastTyper = true;
    }
}