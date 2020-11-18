using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;
using WzWeb.Shared.Character;

namespace WzWeb.Client.Pages
{
    public partial class CharacterPage : IDisposable, IDisplaySpinner
    {
        #region Managers
        private IBodyComponentManager BodyManager;
        #endregion

        #region Canvas
        private Canvas2DContext context;
        private BECanvasComponent canvasReference;
        #endregion

        #region 角色部位图片引用
        private ElementReference HeadRef;
        private ElementReference BodyRef;
        private ElementReference ArmRef;
        private ElementReference FaceRef;
        private ElementReference HandRef;
        private ElementReference EarRef;
        private ElementReference HairRef;
        private ElementReference HairOverHeadRef;
        private ElementReference HairBelowBodyRef;
        private ElementReference MailRef;
        private ElementReference PantsRef;
        private ElementReference BackHairRef;
        private ElementReference BackHairBelowCapRef;
        private ElementReference MailArmRef;
        #endregion

        #region 后台数据
        private IList<int> Skins;
        private IList<string> Actions;
        private IList<int> Frames;
        #endregion

        private Character character;
        private int currentSkin;
        private string currentAction;
        private EarType currentEarType;
        private ConfigType currentNavConfigType = ConfigType.Face;

        #region 计时器相关
        private Timer anitimer;
        private Timer faceAniTimer;
        #endregion

        private int _currentFrame;
        private int _currentFaceFrame;

        private int CurrentFrame
        {
            get
            {
                return _currentFrame;
            }
            set
            {
                if (value > Frames.Count - 1) value = 0;
                _currentFrame = value;
                BrowserService.CurrentCharacter.CurrentFrame = _currentFrame.ToString();
                character = BrowserService.CurrentCharacter;
                StateHasChanged();
            }
        }

        private int CurrentFaceFrame
        {
            get
            {
                return _currentFaceFrame;
            }
            set
            {
                if (value > character.CurrentFaceMotion.FrameCount - 1) value = 0;
                _currentFaceFrame = value;
                BrowserService.CurrentCharacter.CurrentFaceFrame = _currentFaceFrame.ToString();
                character = BrowserService.CurrentCharacter;
                StateHasChanged();
            }
        }


        public bool DisplaySpinner => character == null || Skins == null || Actions == null || Frames == null;
        private bool loading = true;

        protected async override Task OnInitializedAsync()
        {
            Notifier.Notify += OnNotify;
            BodyManager = BrowserService.GetBodyComponentManager<Face>();
            anitimer = new Timer(new TimerCallback(Animate), null, Timeout.Infinite, 60);
            Skins = await BrowserService.GetSkins();
            character = await BrowserService.GetDefaultCharacter();
            currentSkin = character.Id;
            currentAction = character.CurrentHeadMotion.Name;
            faceAniTimer = new Timer(new TimerCallback(FaceAnimate), null, 100, character.FaceDelay);
            await RefreshDataAsync(true);
        }

        private async Task OnNotify(string key, int value)
        {
            await InvokeAsync(async () =>
            {
                await RefreshDataAsync();
                StateHasChanged();
            });
        }

        protected async Task RefreshDataAsync(bool isFirst = false)
        {
            loading = true;
            StopAnimation();
            Actions = await BrowserService.GetActions(character.Id);
            if (!isFirst)
            {
                character = await BrowserService.GetCharacterAsync(currentSkin, currentAction, 0);
            }
            Frames = character.CurrentBodyMotion.Actions.Keys.Select(key => int.Parse(key)).ToList();
            CurrentFrame = 0;
            CurrentFaceFrame = 0;
            character.EarType = currentEarType;

            PlayAnimation();
            loading = false;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (character == null) return;
            await BuildCanvas();
        }

        #region 换装相关函数
        private async Task ChangeSkin(ChangeEventArgs eventArgs)
        {
            currentSkin = int.Parse(eventArgs.Value.ToString());
            await RefreshDataAsync();
        }
        private async Task ChangeAction(ChangeEventArgs eventArgs)
        {
            currentAction = eventArgs.Value.ToString();
            await RefreshDataAsync();
        }
        private async Task ChangeEar(ChangeEventArgs eventArgs)
        {
            currentEarType = (EarType)(int.Parse(eventArgs.Value.ToString()));
            await RefreshDataAsync();
        }
        private async Task ChangeBodyNav(ConfigType configType)
        {
            await InvokeAsync(() =>
            {
                switch (configType)
                {

                    case ConfigType.Face:
                        BodyManager = BrowserService.GetBodyComponentManager<Face>();
                        break;
                    case ConfigType.Hair:
                        BodyManager = BrowserService.GetBodyComponentManager<Hair>();
                        break;
                    case ConfigType.Coat:
                        BodyManager = BrowserService.GetBodyComponentManager<Coat>();
                        break;
                    case ConfigType.Pants:
                        BodyManager = BrowserService.GetBodyComponentManager<Pants>();
                        break;
                    case ConfigType.Head:
                    case ConfigType.Body:
                    default:
                        break;
                }
                StateHasChanged();
            });
        }
        #endregion

        private async Task BuildCanvas()
        {
            if (context == null)
            {
                context = await canvasReference.CreateCanvas2DAsync();
                await context.SetTextAlignAsync(TextAlign.Center);
                await context.SetTextBaselineAsync(TextBaseline.Middle);
            }

            await context.ClearRectAsync(0, 0, 150, 150);

            if (loading)
            {
                await context.FillTextAsync("Loading...", 75, 75);
                return;
            }

            Point drawPosition;

            if (character.HairBelowBody != null)
            {
                drawPosition = CalculateDrawPosition(character.HairBelowBodyPosition, character.HairBelowBody);
                await context.DrawImageAsync(HairBelowBodyRef, drawPosition.X, drawPosition.Y);
            }

            drawPosition = CalculateDrawPosition(character.BodyPosition, character.Body);
            await context.DrawImageAsync(BodyRef, drawPosition.X, drawPosition.Y);

            if (character.Mail != null)
            {
                drawPosition = CalculateDrawPosition(character.MailPosition, character.Mail);
                await context.DrawImageAsync(MailRef, drawPosition.X, drawPosition.Y);
            }


            drawPosition = CalculateDrawPosition(character.HeadPosition, character.Head);
            await context.DrawImageAsync(HeadRef, drawPosition.X, drawPosition.Y);

            if (character.EarType != EarType.Normal)
            {
                drawPosition = CalculateDrawPosition(character.EarPosition, character.Ear);
                await context.DrawImageAsync(EarRef, drawPosition.X, drawPosition.Y);
            }
            if (character.HasFace)
            {
                drawPosition = CalculateDrawPosition(character.FacePosition, character.Face);
                await context.DrawImageAsync(FaceRef, drawPosition.X, drawPosition.Y);
            }
            if (character.Hair != null)
            {
                drawPosition = CalculateDrawPosition(character.HairPosition, character.Hair);
                await context.DrawImageAsync(HairRef, drawPosition.X, drawPosition.Y);
            }
            if (character.HairOverHead != null)
            {
                drawPosition = CalculateDrawPosition(character.HairOverHeadPosition, character.HairOverHead);
                await context.DrawImageAsync(HairOverHeadRef, drawPosition.X, drawPosition.Y);
            }
            if (character.BackHair != null)
            {
                drawPosition = CalculateDrawPosition(character.BackHairPosition, character.BackHair);
                await context.DrawImageAsync(BackHairRef, drawPosition.X, drawPosition.Y);
            }
            if (character.BackHairBelowCap != null)
            {
                drawPosition = CalculateDrawPosition(character.BackHairBelowCapPosition, character.BackHairBelowCap);
                await context.DrawImageAsync(BackHairBelowCapRef, drawPosition.X, drawPosition.Y);
            }

            if (character.Pants != null)
            {
                drawPosition = CalculateDrawPosition(character.PantsPosition, character.Pants);
                await context.DrawImageAsync(PantsRef, drawPosition.X, drawPosition.Y);
            }
            if (character.Arm != null)
            {
                drawPosition = CalculateDrawPosition(character.ArmPosition, character.Arm);
                await context.DrawImageAsync(ArmRef, drawPosition.X, drawPosition.Y);
            }
            if (character.Hand != null)
            {
                drawPosition = CalculateDrawPosition(character.HandPosition, character.Hand);
                await context.DrawImageAsync(HandRef, drawPosition.X, drawPosition.Y);
            }
            if (character.MailArm != null)
            {
                drawPosition = CalculateDrawPosition(character.MailArmPosition, character.MailArm);
                await context.DrawImageAsync(MailArmRef, drawPosition.X, drawPosition.Y);
            }

        }

        private Point CalculateDrawPosition(Point srcPosition, CharacterConfig config)
        {
            var x = srcPosition.X - config.Origin.X;
            var y = srcPosition.Y - config.Origin.Y;
            return new Point(x, y);
        }


        #region 动画相关
        private void PlayAnimation()
        {
            anitimer.Change(0, character.BodyDelay);
        }
        private void StopAnimation()
        {
            anitimer.Change(Timeout.Infinite, character.BodyDelay);
        }
        /// <summary>
        ///计时器回调
        /// </summary>
        private void Animate(object value)
        {
            InvokeAsync(() =>
            {
                CurrentFrame++;
            });
        }
        private void FaceAnimate(object value)
        {
            InvokeAsync(() =>
            {
                if (character == null) return;

                CurrentFaceFrame++;
                if (CurrentFaceFrame == character.CurrentFaceMotion.FrameCount - 1)
                {
                    CurrentFaceFrame++;
                    faceAniTimer.Change(1500, character.FaceDelay);
                }
            });
        }
        #endregion

        public void Dispose()
        {
            anitimer.Change(Timeout.Infinite, 100);
            faceAniTimer.Change(Timeout.Infinite, 100);
            anitimer.Dispose();
            faceAniTimer.Dispose();
            Notifier.Notify -= OnNotify;
        }
    }
}
