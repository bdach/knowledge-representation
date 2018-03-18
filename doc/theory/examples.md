# Historyjki na RW - draft

## Stół i wazon

Fluenty:

* *leftSideUp*
* *rightSideUp*
* *bothSidesUp*
* *vaseBroken*

Akcje:

* raiseLeftSide
* raiseRightSide
* dropLeftSide
* dropRightSide

**initially** ~*leftSideUp*

**initially** ~*rightSideUp*

**initially** ~*vaseBroken*

raiseLeftSide **causes** *leftSideUp* **if** ~*leftSideUp*

raiseRightSide **causes** *rightSideUp* **if** ~*rightSideUp*

dropLeftSide **causes** ~*leftSideUp* **if** *leftSideUp*

dropRightSide **causes** ~*rightSideUp* **if** *rightSideUp*

**once** ~(*leftSideUp* <=> *rightSideUp*) **always** *vaseBroken*

### Jak uwiązać fluent

* **once** *f* **always** *g*

  po pierwszym spełnieniu *f* zawsze zachodzi *g*

  sprawdzamy po każdej jednostce czasu

## Trzech filozofów

Fluenty:

* *philosopher{1,2,3}has{Left,Right}Fork*
* *philosopher1ate*
* *philosopher2ate*
* *philosopher3ate*

Akcje:

* philosopher{1,2,3}pickUpLeft
* philosopher{1,2,3}dropLeft
* philosopher{1,2,3}pickUpRight
* philosopher{1,2,3}dropRight

**initially** ~*fork{1,2,3}taken*

philosopher1pickUpLeft **causes** *philosopher1hasLeftFork* **if** ~*philosopher3hasRightFork*

philosopher1pickUpRight **causes** *philosopher1hasRightFork* **if** ~*philosopher2hasLeftFork*

(...)

**once** (*philosopher1hasLeftFork* and *philosopher1hasRightFork*) **always** *philosopher1ate*

## Producenci i konsumenci

Fluenty:

* *bufferEmpty*
* *AhasItem*
* *BhasItem*

Akcje:

* put
* consumeA
* getA
* consumeB
* getB

**initially** *bufferEmpty*

**initially** ~*AhasItem*

**initially** ~*BhasItem*

put **causes** ~*bufferEmpty* **if** *bufferEmpty*

getA **causes** (*AhasItem* and *bufferEmpty*) **if** (~*AhasItem* and ~*bufferEmpty*)

getB **causes** (*BhasItem* and *bufferEmpty*) **if** (~*BhasItem* and ~*bufferEmpty*)

consumeA **causes** ~*AhasItem* **if** *AhasItem*

consumeB **causes** ~*BhasItem* **if** *BhasItem*

# Pytania

* zdania **after** z ciągiem akcji - jak rozważać przy akcjach współbieżnych?
* zdania **after** i **causes** w jednej "jednostce czasu" - jak je rozważać?
* czy efekty akcji są natychmiastowe? czy czekamy na inne akcje? ustalamy jakąś kolejność ich wykonywania?
* jak ustalić fluent tak aby już się nie zmieniał? => czy nasze **once...always** jest akceptowalne?