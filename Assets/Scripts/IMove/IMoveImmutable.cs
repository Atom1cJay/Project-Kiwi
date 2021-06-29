using System;

/// <summary>
/// Provides access to an IMove object, without the ability to mutate its
/// contents or modify its state in any way.
/// </summary>
public interface IMoveImmutable
{
    /// <summary>
    /// Gives the horizontal speed the player should be at this frame for
    /// this move, based on the current simulated time this frame.
    /// </summary>
    float GetHorizSpeedThisFrame();

    /// <summary>
    /// Gives the vertical speed the player should be at this frame for
    /// this move, based on the current simulated time in this move.
    /// </summary>
    float GetVertSpeedThisFrame();

    /// <summary>
    /// Gives the rotation speed at which the player should be moving this
    /// frame.
    /// </summary>
    float GetRotationSpeed();

    /// <summary>
    /// Determines whether this move being initiated should bring up the triple
    /// jump counter.
    /// </summary>
    bool IncrementsTJcounter();

    /// <summary>
    /// Determines whether a triple jump cycle, if it is currently happening,
    /// should be broken. (In other words, should the triple jump count
    /// be reset right now?)
    /// </summary>
    bool TJshouldBreak();

    /// <summary>
    /// Gives this move as a String with no spaces, all in lowercase.
    /// </summary>
    /// <returns></returns>
    string AsString();

    /// <summary>
    /// What move should start being performed next frame?
    /// </summary>
    IMove GetNextMove();
}
