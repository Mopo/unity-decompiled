﻿// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Describes information and value of a single shader property.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class MaterialProperty
  {
    private UnityEngine.Object[] m_Targets;
    private MaterialProperty.ApplyPropertyCallback m_ApplyPropertyCallback;
    private string m_Name;
    private string m_DisplayName;
    private object m_Value;
    private Vector4 m_TextureScaleAndOffset;
    private Vector2 m_RangeLimits;
    private MaterialProperty.PropType m_Type;
    private MaterialProperty.PropFlags m_Flags;
    private MaterialProperty.TexDim m_TextureDimension;
    private int m_MixedValueMask;

    /// <summary>
    ///   <para>Material objects being edited by this property (Read Only).</para>
    /// </summary>
    public UnityEngine.Object[] targets
    {
      get
      {
        return this.m_Targets;
      }
    }

    /// <summary>
    ///   <para>Type of the property (Read Only).</para>
    /// </summary>
    public MaterialProperty.PropType type
    {
      get
      {
        return this.m_Type;
      }
    }

    /// <summary>
    ///   <para>Name of the property (Read Only).</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>Display name of the property (Read Only).</para>
    /// </summary>
    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    /// <summary>
    ///   <para>Flags that control how property is displayed (Read Only).</para>
    /// </summary>
    public MaterialProperty.PropFlags flags
    {
      get
      {
        return this.m_Flags;
      }
    }

    /// <summary>
    ///   <para>Texture dimension (2D, Cubemap etc.) of the property (Read Only).</para>
    /// </summary>
    public MaterialProperty.TexDim textureDimension
    {
      get
      {
        return this.m_TextureDimension;
      }
    }

    /// <summary>
    ///   <para>Min/max limits of a ranged float property (Read Only).</para>
    /// </summary>
    public Vector2 rangeLimits
    {
      get
      {
        return this.m_RangeLimits;
      }
    }

    /// <summary>
    ///   <para>Does this property have multiple different values? (Read Only)</para>
    /// </summary>
    public bool hasMixedValue
    {
      get
      {
        return (this.m_MixedValueMask & 1) != 0;
      }
    }

    public MaterialProperty.ApplyPropertyCallback applyPropertyCallback
    {
      get
      {
        return this.m_ApplyPropertyCallback;
      }
      set
      {
        this.m_ApplyPropertyCallback = value;
      }
    }

    internal int mixedValueMask
    {
      get
      {
        return this.m_MixedValueMask;
      }
    }

    /// <summary>
    ///   <para>Color value of the property.</para>
    /// </summary>
    public Color colorValue
    {
      get
      {
        if (this.m_Type == MaterialProperty.PropType.Color)
          return (Color) this.m_Value;
        return Color.black;
      }
      set
      {
        if (this.m_Type != MaterialProperty.PropType.Color || !this.hasMixedValue && value == (Color) this.m_Value)
          return;
        this.ApplyProperty((object) value);
      }
    }

    /// <summary>
    ///   <para>Vector value of the property.</para>
    /// </summary>
    public Vector4 vectorValue
    {
      get
      {
        if (this.m_Type == MaterialProperty.PropType.Vector)
          return (Vector4) this.m_Value;
        return Vector4.zero;
      }
      set
      {
        if (this.m_Type != MaterialProperty.PropType.Vector || !this.hasMixedValue && value == (Vector4) this.m_Value)
          return;
        this.ApplyProperty((object) value);
      }
    }

    /// <summary>
    ///   <para>Float vaue of the property.</para>
    /// </summary>
    public float floatValue
    {
      get
      {
        if (this.m_Type == MaterialProperty.PropType.Float || this.m_Type == MaterialProperty.PropType.Range)
          return (float) this.m_Value;
        return 0.0f;
      }
      set
      {
        if (this.m_Type != MaterialProperty.PropType.Float && this.m_Type != MaterialProperty.PropType.Range || !this.hasMixedValue && (double) value == (double) (float) this.m_Value)
          return;
        this.ApplyProperty((object) value);
      }
    }

    /// <summary>
    ///   <para>Texture value of the property.</para>
    /// </summary>
    public Texture textureValue
    {
      get
      {
        if (this.m_Type == MaterialProperty.PropType.Texture)
          return (Texture) this.m_Value;
        return (Texture) null;
      }
      set
      {
        if (this.m_Type != MaterialProperty.PropType.Texture || !this.hasMixedValue && (UnityEngine.Object) value == (UnityEngine.Object) this.m_Value)
          return;
        this.m_MixedValueMask &= -2;
        object previousValue = this.m_Value;
        this.m_Value = (object) value;
        this.ApplyProperty(previousValue, 1);
      }
    }

    public Vector4 textureScaleAndOffset
    {
      get
      {
        if (this.m_Type == MaterialProperty.PropType.Texture)
          return this.m_TextureScaleAndOffset;
        return Vector4.zero;
      }
      set
      {
        if (this.m_Type != MaterialProperty.PropType.Texture || !this.hasMixedValue && value == this.m_TextureScaleAndOffset)
          return;
        this.m_MixedValueMask &= 1;
        int changedPropertyMask = 0;
        for (int index = 1; index < 5; ++index)
          changedPropertyMask |= 1 << index;
        object textureScaleAndOffset = (object) this.m_TextureScaleAndOffset;
        this.m_TextureScaleAndOffset = value;
        this.ApplyProperty(textureScaleAndOffset, changedPropertyMask);
      }
    }

    public void ReadFromMaterialPropertyBlock(MaterialPropertyBlock block)
    {
      ShaderUtil.ApplyMaterialPropertyBlockToMaterialProperty(block, this);
    }

    public void WriteToMaterialPropertyBlock(MaterialPropertyBlock materialblock, int changedPropertyMask)
    {
      ShaderUtil.ApplyMaterialPropertyToMaterialPropertyBlock(this, changedPropertyMask, materialblock);
    }

    internal static bool IsTextureOffsetAndScaleChangedMask(int changedMask)
    {
      changedMask >>= 1;
      return changedMask != 0;
    }

    private void ApplyProperty(object newValue)
    {
      this.m_MixedValueMask = 0;
      object previousValue = this.m_Value;
      this.m_Value = newValue;
      this.ApplyProperty(previousValue, 1);
    }

    private void ApplyProperty(object previousValue, int changedPropertyMask)
    {
      if (this.targets == null || this.targets.Length == 0)
        throw new ArgumentException("No material targets provided");
      UnityEngine.Object[] targets = this.targets;
      string str;
      if (targets.Length == 1)
        str = targets[0].name;
      else
        str = targets.Length.ToString() + " " + ObjectNames.NicifyVariableName(ObjectNames.GetClassName(targets[0])) + "s";
      bool flag = false;
      if (this.m_ApplyPropertyCallback != null)
        flag = this.m_ApplyPropertyCallback(this, changedPropertyMask, previousValue);
      if (flag)
        return;
      ShaderUtil.ApplyProperty(this, changedPropertyMask, "Modify " + this.displayName + " of " + str);
    }

    /// <summary>
    ///   <para>Material property type.</para>
    /// </summary>
    public enum PropType
    {
      Color,
      Vector,
      Float,
      Range,
      Texture,
    }

    /// <summary>
    ///   <para>Texture dimension of a property.</para>
    /// </summary>
    public enum TexDim
    {
      Unknown = -1,
      None = 0,
      Tex2D = 2,
      Tex3D = 3,
      Cube = 4,
      Any = 6,
    }

    /// <summary>
    ///   <para>Flags that control how a MaterialProperty is displayed.</para>
    /// </summary>
    [System.Flags]
    public enum PropFlags
    {
      None = 0,
      HideInInspector = 1,
      PerRendererData = 2,
      NoScaleOffset = 4,
      Normal = 8,
      HDR = 16,
    }

    public delegate bool ApplyPropertyCallback(MaterialProperty prop, int changeMask, object previousValue);
  }
}
