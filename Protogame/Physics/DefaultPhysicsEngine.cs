using Jitter;
using Jitter.Dynamics;

namespace Protogame
{
    public class DefaultPhysicsEngine : IPhysicsEngine
    {
        private readonly IPhysicsFactory _physicsFactory;
        private IWorld _currentWorld;
        private IServerWorld _currentServerWorld;
        private PhysicsShadowWorld _shadowWorld;

        public DefaultPhysicsEngine(IPhysicsFactory physicsFactory)
        {
            _physicsFactory = physicsFactory;
        }
        
        public void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            if (gameContext.World != _currentWorld || _shadowWorld == null)
            {
                if (_shadowWorld != null)
                {
                    _shadowWorld.Dispose();
                }

                _shadowWorld = _physicsFactory.CreateShadowWorld();
                _currentWorld = gameContext.World;
            }

            _shadowWorld.Update(gameContext, updateContext);
        }

        public void Update(IServerContext serverContext, IUpdateContext updateContext)
        {
            if (serverContext.World != _currentServerWorld || _shadowWorld == null)
            {
                if (_shadowWorld != null)
                {
                    _shadowWorld.Dispose();
                }

                _shadowWorld = _physicsFactory.CreateShadowWorld();
                _currentServerWorld = serverContext.World;
            }

            _shadowWorld.Update(serverContext, updateContext);
        }

        public void RegisterRigidBodyForHasMatrixInCurrentWorld(RigidBody rigidBody, IHasTransform hasTransform)
        {
            _shadowWorld.RegisterRigidBodyForHasMatrix(rigidBody, hasTransform);
        }

        public void UnregisterRigidBodyForHasMatrixInCurrentWorld(RigidBody rigidBody, IHasTransform hasTransform)
        {
            _shadowWorld.UnregisterRigidBodyForHasMatrix(rigidBody, hasTransform);
        }

        public void DebugRender(IGameContext gameContext, IRenderContext renderContext)
        {
            _shadowWorld.Render(gameContext, renderContext);
        }

        public JitterWorld GetInternalPhysicsWorld()
        {
            return _shadowWorld.GetJitterWorld();
        }
    }
}