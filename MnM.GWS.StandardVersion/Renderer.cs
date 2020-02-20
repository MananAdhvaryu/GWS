using System;
using System.Runtime.CompilerServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS.StandardVersion
{
#if AllHidden
    partial class NativeFactory
    {
#else
    public
#endif
        sealed class Renderer : GwsRenderer, IRenderer
        {
            #region RENDER PIXEL - LINE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe override void RenderPixel(IBuffer target, int index, int color, bool blend, float? delta = null)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                if (index < 0 || index >= target.Length)
                    return;

                target.XYOf(index, out int x, out int y);

                int i = index;

                if (blend && FillMode != FillMode.Erase)
                {
                    if (delta != null)
                        target[i] = Colours.Blend(target[i], color, delta.Value);
                    else
                        target[i] = Colours.Blend(target[i], color);
                }
                else
                    target[i] = color;

                NotifyDrawPoint(x, y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe override void RenderLine(IBuffer target, IBufferPen reader, int destVal, int destAxis, int start, int end, int axis, bool horizontal, float? delta = null)
            {
                if (!GetAxisLineInfo(target, ref destVal, ref destAxis, ref start, ref end, horizontal, out int destIndex, out int length))
                    return;

                ReadLine(reader, start, (start + length), axis, horizontal, out IntPtr source, out int sIndex, out length);

                if (length == 0)
                    return;

                int* src = (int*)source;

                float? alpha;

                if (FillMode == FillMode.Erase)
                    alpha = null;
                else
                    alpha = delta;

                if (IsRotated(Entity.Buffer))
                {
                    var j = sIndex;
                    if (length < 2)
                        return;
                    var last = destVal + length - 1;
                    for (float i = destVal; i <= last; i++)
                    {
                        bool blend = i == start || i == last;
                        target.WritePixel(i, destAxis, horizontal, src[j], blend);
                        j++;
                    }
                    return;
                }

                int* dest = (int*)target.Pixels;
                if (horizontal)
                {
                    if (alpha != null)
                        Colours.Blend(src, sIndex, length, dest, destIndex, alpha);
                    else
                        CopyMemory(src, sIndex, dest, destIndex, length);

                    NotifyAxisLine(destVal, destAxis, horizontal, length);
                }
                else
                {
                    if (alpha != null)
                    {
                        for (int i = sIndex; i < sIndex + length; i++)
                        {
                            dest[destIndex] = Colours.Blend(dest[destIndex], src[i], alpha.Value);
                            destIndex += target.Width;
                        }
                    }
                    else
                    {
                        for (int i = sIndex; i < sIndex + length; i++)
                        {
                            dest[destIndex] = src[i];
                            destIndex += target.Width;
                        }
                    }
                    NotifyAxisLine(destVal, destAxis, horizontal, length);
                }

            }
            #endregion

            #region COPY MEMORY
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override unsafe void CopyMemory(int* src, int srcIndex, int* dst, int destIndex, int length)
            {
                src += srcIndex;
                dst += destIndex;
                var end = dst + length;
                while (dst != end)
                {
                    *dst++ = *src++;
                }
            }
            #endregion

            #region RENDER
            public override void Render(IBuffer Buffer, IElement shape, IPenContext context)
            {
                if (Buffer == null || ShapeBeingDrawn == shape.ID)
                    return;
                ShapeBeingDrawn = shape.ID;

                bool AddMode = false;
                bool Exists = false;

                IDrawInfo3 info = null;

                if(Buffer is IContainer)
                {
                    var path = (Buffer as IContainer).Controls;
                    AddMode = path.AddMode;
                    Exists = path.Contains(shape);

                    if (!AddMode && !Exists)
                    {
                        RenderElement(Buffer, shape, context);
                        ShapeBeingDrawn = null;
                        return;
                    }
                    AddMode = AddMode  && !Exists;

                    if (AddMode)
                        info = path.NewDrawInfo(shape);
                    else
                        info = path[shape];
                }

                IPenContext Pen = context;

                if (AddMode)
                {
                    Factory.Add(shape, ObjType.Element);
                    if (shape is ILine && shape.ID == null)
                        (shape as ILine).AssignIDIfNone();

                    info.CopyFrom(this);
                }
                else if (Exists)
                {
                    if (context == null && info.Reader != null)
                        Pen = Factory.Get<IBufferPen>(info.Reader, ObjType.Buffer);
                }

                RenderElement(Buffer, shape, Pen);
                ShapeBeingDrawn = null;
            }
            public override bool RenderCustom(IBuffer Buffer, IElement element, IPenContext context, out IBufferPen reader)
            {
                reader = null;
                return false;
            }
            #endregion
        }
#if AllHidden
    }
#endif
}
